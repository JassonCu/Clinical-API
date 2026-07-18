using Clinical.Web.Core.DTOs.Auth;

namespace Clinical.Web.Core.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto?> LoginAsync(LoginRequestDto request);
    Task<AuthResponseDto?> RefreshTokenAsync(string refreshToken);
    Task<bool> RegisterAsync(RegisterRequestDto request);
    Task<(bool Success, string? Error)> ResetPasswordAsync(ResetPasswordDto dto);
    Task<(bool Success, string? Error)> ChangePasswordAsync(int userId, string newPassword);
}
