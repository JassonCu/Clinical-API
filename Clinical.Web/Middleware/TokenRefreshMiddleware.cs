using System.Security.Claims;
using Clinical.Web.Core.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Clinical.Web.Middleware;

public class TokenRefreshMiddleware
{
    private readonly RequestDelegate _next;

    public TokenRefreshMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context, IAuthService authService)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var expiryStr = context.User.FindFirstValue("token_expiry");
            if (DateTime.TryParse(expiryStr, out var expiry) && DateTime.UtcNow >= expiry.AddMinutes(-2))
            {
                var refreshToken = context.Request.Cookies["X-Clinical-Refresh"];
                if (!string.IsNullOrEmpty(refreshToken))
                {
                    var result = await authService.RefreshTokenAsync(refreshToken);
                    if (result is not null)
                    {
                        SetTokenCookies(context, result.AccessToken, result.RefreshToken, result.ExpiresAt);

                        var claims = BuildClaims(result.Username, result.Email, result.FullName, result.Role, result.ExpiresAt, result.UserId, result.MustChangePassword);
                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(identity),
                            new AuthenticationProperties { IsPersistent = true, ExpiresUtc = result.ExpiresAt });
                    }
                    else
                    {
                        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        context.Response.Redirect("/Auth/Login");
                        return;
                    }
                }
            }
        }

        await _next(context);
    }

    private static void SetTokenCookies(HttpContext context, string access, string refresh, DateTime expiry)
    {
        var baseOpts = new CookieOptions
        {
            HttpOnly = true,
            Secure = context.Request.IsHttps,
            SameSite = SameSiteMode.Strict
        };
        context.Response.Cookies.Append("X-Clinical-Token", access, new CookieOptions
        {
            HttpOnly = baseOpts.HttpOnly,
            Secure = baseOpts.Secure,
            SameSite = baseOpts.SameSite,
            Expires = expiry
        });
        context.Response.Cookies.Append("X-Clinical-Refresh", refresh, new CookieOptions
        {
            HttpOnly = baseOpts.HttpOnly,
            Secure = baseOpts.Secure,
            SameSite = baseOpts.SameSite,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        });
    }

    internal static IEnumerable<Claim> BuildClaims(string username, string email, string fullName, string role, DateTime expiry, int userId, bool mustChangePassword) =>
    [
        new(ClaimTypes.NameIdentifier, username),
        new(ClaimTypes.Name, username),
        new(ClaimTypes.Email, email),
        new("full_name", fullName),
        new(ClaimTypes.Role, role),
        new("token_expiry", expiry.ToString("O")),
        new("user_id", userId.ToString()),
        new("must_change_password", mustChangePassword.ToString().ToLower())
    ];
}
