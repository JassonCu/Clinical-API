using Clinical.Application.DTOS.Auth.Response;
using Clinical.Infraestructure.Services;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.Auth.Commands.LoginCommand;

public class LoginHandler : IRequestHandler<LoginCommand, BaseResponse<AuthResponseDto>>
{
    private readonly IAuthRepository _authRepository;
    private readonly JwtTokenService _jwtTokenService;

    public LoginHandler(IAuthRepository authRepository, JwtTokenService jwtTokenService)
    {
        _authRepository = authRepository;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<BaseResponse<AuthResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<AuthResponseDto>();

        try
        {
            var user = await _authRepository.GetUserByUsernameAsync(request.Username!);

            if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                response.IsSuccess = false;
                response.Message = GlobalMessage.MESSAGE_TOKEN_ERROR;
                return response;
            }

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

            await _authRepository.UpdateRefreshTokenAsync(user.UserId.Value, refreshToken, refreshExpiry);

            response.IsSuccess = true;
            response.Message = GlobalMessage.MESSAGE_TOKEN;
            response.Data = new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15),
                Username = user.Username,
                Email = user.Email,
                FullName = $"{user.FirstName} {user.LastName}",
                Role = roleName
            };
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}