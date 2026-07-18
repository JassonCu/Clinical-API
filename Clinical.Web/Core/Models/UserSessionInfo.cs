namespace Clinical.Web.Core.Models;

public class UserSessionInfo
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }

    public bool IsTokenExpiring(int bufferMinutes = 2) =>
        DateTime.UtcNow >= ExpiresAt.AddMinutes(-bufferMinutes);
}
