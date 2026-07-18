using Clinical.Application.DTOS.Auth.Response;
using Clinical.Infraestructure.Services;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.Auth.Commands.RefreshTokenCommand;

public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, BaseResponse<AuthResponseDto>>
{
    private readonly IAuthRepository _authRepository;
    private readonly Infraestructure.Services.IJwtTokenService _jwtTokenService;

    public RefreshTokenHandler(IAuthRepository authRepository, IJwtTokenService jwtTokenService)
    {
        _authRepository = authRepository;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<BaseResponse<AuthResponseDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<AuthResponseDto>();

        var user = await _authRepository.GetUserByRefreshTokenAsync(request.RefreshToken!);

        if (user is null || user.RefreshTokenExpiry < DateTime.UtcNow)
        {
            response.IsSuccess = false;
            response.Message = GlobalMessage.MESSAGE_REFRESH_TOKEN_INVALID;
            return response;
        }

        var roleName = await _authRepository.GetRoleNameAsync(user.RoleId!.Value) ?? "User";
        var newAccessToken = _jwtTokenService.GenerateAccessToken(user.UserId!.Value, user.Username!, user.Email!, roleName);
        var newRefreshToken = _jwtTokenService.GenerateRefreshToken();
        var refreshExpiry = DateTime.UtcNow.AddDays(7);

        await _authRepository.UpdateRefreshTokenAsync(user.UserId.Value, newRefreshToken, refreshExpiry);

        response.IsSuccess = true;
        response.Message = GlobalMessage.MESSAGE_REFRESH_TOKEN_SUCCESS;
        response.Data = new AuthResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(15),
            Username = user.Username,
            Email = user.Email,
            FullName = $"{user.FirstName} {user.LastName}",
            Role = roleName
        };

        return response;
    }
}