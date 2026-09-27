using Clinical.Application.DTOS.Auth.Response;
using Clinical.Infraestructure.Services;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using Clinical.Utils.Security;
using MediatR;

namespace Clinical.UseCases.UseCases.Auth.Commands.LoginCommand;

public class LoginHandler : IRequestHandler<LoginCommand, BaseResponse<AuthResponseDto>>
{
    private readonly IAuthRepository _authRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILoginAttemptTracker _attempts;

    // Precomputed hash so BCrypt.Verify runs even when the username does not exist,
    // keeping response time comparable and preventing username enumeration by timing.
    private static readonly string DummyPasswordHash =
        BCrypt.Net.BCrypt.HashPassword("timing-attack-mitigation", workFactor: 12);

    public LoginHandler(IAuthRepository authRepository, IJwtTokenService jwtTokenService, ILoginAttemptTracker attempts)
    {
        _authRepository = authRepository;
        _jwtTokenService = jwtTokenService;
        _attempts = attempts;
    }

    public async Task<BaseResponse<AuthResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<AuthResponseDto>();

        // Temporary lockout after repeated failures (complements the per-IP rate limiter).
        // Keyed on the submitted username so it never reveals whether the account exists.
        if (_attempts.IsLockedOut(request.Username!))
        {
            response.IsSuccess = false;
            response.Message = GlobalMessage.MESSAGE_ACCOUNT_LOCKED;
            return response;
        }

        var user = await _authRepository.GetUserByUsernameAsync(request.Username!);

        // Always run one verification (against a dummy hash when the user is missing) so a
        // non-existent username and a wrong password cost the same time and return the same message.
        var hashToVerify = string.IsNullOrEmpty(user?.PasswordHash) ? DummyPasswordHash : user.PasswordHash;
        var passwordValid = BCrypt.Net.BCrypt.Verify(request.Password, hashToVerify);

        if (user is null || !passwordValid)
        {
            _attempts.RegisterFailure(request.Username!);
            response.IsSuccess = false;
            response.Message = GlobalMessage.MESSAGE_TOKEN_ERROR;
            return response;
        }

        // Correct credentials → clear any accumulated failures.
        _attempts.Reset(request.Username!);

        if (user.State != 1)
        {
            response.IsSuccess = false;
            response.Message = "El usuario se encuentra inactivo.";
            return response;
        }

        var roleName = await _authRepository.GetRoleNameAsync(user.RoleId!.Value) ?? "User";
        var accessToken = _jwtTokenService.GenerateAccessToken(user.UserId!.Value, user.Username!, user.Email!, roleName);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();
        var refreshExpiry = DateTime.UtcNow.AddDays(7);

        // Persist only the hash; the client keeps the raw token returned below.
        await _authRepository.UpdateRefreshTokenAsync(user.UserId.Value, TokenHasher.Hash(refreshToken), refreshExpiry);

        response.IsSuccess = true;
        response.Message = GlobalMessage.MESSAGE_TOKEN;
        response.Data = new AuthResponseDto
        {
            UserId = user.UserId!.Value,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(15),
            Username = user.Username,
            Email = user.Email,
            FullName = $"{user.FirstName} {user.LastName}",
            Role = roleName,
            MustChangePassword = user.MustChangePassword ?? false
        };

        return response;
    }
}