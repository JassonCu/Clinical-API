using System.Security.Claims;

namespace Clinical.Infraestructure.Services
{
    public interface IJwtTokenService
    {
        string GenerateAccessToken(int userId, string username, string email, string role);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
