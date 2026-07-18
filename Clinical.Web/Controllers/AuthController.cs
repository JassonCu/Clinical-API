using System.Security.Claims;
using Clinical.Web.Core.DTOs.Auth;
using Clinical.Web.Core.Interfaces;
using Clinical.Web.Middleware;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Clinical.Web.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly ISetupService _setupService;
    private readonly ILogger<AuthController> _logger;

    private int CurrentUserId => int.TryParse(User.FindFirstValue("user_id"), out var id) ? id : 0;

    public AuthController(IAuthService authService, ISetupService setupService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _setupService = setupService;
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string? returnUrl = null)
    {
        var isInitialized = await _setupService.IsInitializedAsync();
        if (!isInitialized) return RedirectToAction("Index", "Setup");
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Dashboard");
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [EnableRateLimiting("login")]
    public async Task<IActionResult> Login(LoginRequestDto model, string? returnUrl = null)
    {
        if (!ModelState.IsValid) return View(model);

        var result = await _authService.LoginAsync(model);
        if (result is null)
        {
            ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
            _logger.LogWarning("Failed login attempt for user: {Username}", model.Username);
            return View(model);
        }

        SetTokenCookies(result.AccessToken, result.RefreshToken, result.ExpiresAt);

        var claims = TokenRefreshMiddleware.BuildClaims(
            result.Username, result.Email, result.FullName, result.Role, result.ExpiresAt,
            result.UserId, result.MustChangePassword);
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity),
            new AuthenticationProperties { IsPersistent = true, ExpiresUtc = result.ExpiresAt });

        _logger.LogInformation("User {Username} logged in", result.Username);

        if (result.MustChangePassword)
            return RedirectToAction(nameof(ChangePassword));

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Dashboard");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        DeleteTokenCookies();
        return RedirectToAction("Login");
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult AccessDenied() => View();

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ResetPassword(string? token = null)
    {
        if (User.Identity?.IsAuthenticated == true &&
            User.FindFirstValue("must_change_password") != "true")
            return RedirectToAction("Index", "Dashboard");
        return View(new ResetPasswordDto { Token = token ?? string.Empty });
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
    {
        if (!ModelState.IsValid) return View(model);
        var (success, error) = await _authService.ResetPasswordAsync(model);
        if (success)
        {
            TempData["Success"] = "Contraseña actualizada. Ahora puede iniciar sesión.";
            return RedirectToAction(nameof(Login));
        }
        ModelState.AddModelError(string.Empty, error ?? "Error al restablecer la contraseña.");
        return View(model);
    }

    [HttpGet]
    [Authorize]
    public IActionResult ChangePassword() => View(new ChangePasswordDto());

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
    {
        if (!ModelState.IsValid) return View(model);
        var userId = CurrentUserId;
        var (success, error) = await _authService.ChangePasswordAsync(userId, model.NewPassword);
        if (success)
        {
            // Refresh authentication to clear must_change_password
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["Success"] = "Contraseña actualizada. Por favor inicie sesión nuevamente.";
            return RedirectToAction(nameof(Login));
        }
        ModelState.AddModelError(string.Empty, error ?? "Error al cambiar la contraseña.");
        return View(model);
    }

    private void SetTokenCookies(string access, string refresh, DateTime expiry)
    {
        Response.Cookies.Append("X-Clinical-Token", access, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Secure = Request.IsHttps,
            Expires = expiry
        });
        Response.Cookies.Append("X-Clinical-Refresh", refresh, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Secure = Request.IsHttps,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        });
    }

    private void DeleteTokenCookies()
    {
        Response.Cookies.Delete("X-Clinical-Token");
        Response.Cookies.Delete("X-Clinical-Refresh");
    }
}
