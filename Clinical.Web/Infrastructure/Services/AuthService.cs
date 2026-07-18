using Clinical.Web.Core.DTOs.Auth;
using Clinical.Web.Core.Interfaces;

namespace Clinical.Web.Infrastructure.Services;

public class AuthService : BaseApiService, IAuthService
{
    public AuthService(IHttpClientFactory factory, IHttpContextAccessor accessor, ILogger<AuthService> logger)
        : base(factory, accessor, logger) { }

    public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto request)
    {
        var (success, data, _) = await PostWithResultAsync<LoginRequestDto, AuthResponseDto>("/api/auth/Login", request);
        return success ? data : null;
    }

    public async Task<AuthResponseDto?> RefreshTokenAsync(string refreshToken)
    {
        var (success, data, _) = await PostWithResultAsync<RefreshTokenRequestDto, AuthResponseDto>(
            "/api/auth/RefreshToken", new RefreshTokenRequestDto { RefreshToken = refreshToken });
        return success ? data : null;
    }

    public async Task<bool> RegisterAsync(RegisterRequestDto request)
    {
        var (success, _) = await PostAsync("/api/auth/Register", request);
        return success;
    }

    public async Task<(bool Success, string? Error)> ResetPasswordAsync(ResetPasswordDto dto)
    {
        var (ok, error) = await PostAsync("/api/auth/reset-password", new { Token = dto.Token, NewPassword = dto.NewPassword });
        return (ok, error);
    }

    public async Task<(bool Success, string? Error)> ChangePasswordAsync(int userId, string newPassword)
    {
        var (ok, error) = await PostAsync("/api/auth/change-password", new { UserId = userId, NewPassword = newPassword });
        return (ok, error);
    }
}
