using System.Security.Cryptography;

namespace Clinical.Web.Middleware;

public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;
    private readonly bool _isDevelopment;

    public SecurityHeadersMiddleware(RequestDelegate next, IWebHostEnvironment env)
    {
        _next = next;
        _isDevelopment = env.IsDevelopment();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var nonce = GenerateNonce();
        context.Items["csp-nonce"] = nonce;

        var connectSrc = _isDevelopment
            ? "connect-src 'self' https://cdn.jsdelivr.net ws://localhost:* wss://localhost:*;"
            : "connect-src 'self' https://cdn.jsdelivr.net;";

        var headers = context.Response.Headers;
        headers["X-Content-Type-Options"] = "nosniff";
        headers["X-Frame-Options"] = "DENY";
        headers["X-XSS-Protection"] = "1; mode=block";
        headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
        headers["Permissions-Policy"] = "geolocation=(), microphone=(), camera=()";
        headers["Content-Security-Policy"] =
            "default-src 'self'; " +
            $"script-src 'self' 'nonce-{nonce}' https://cdn.jsdelivr.net https://cdnjs.cloudflare.com https://code.jquery.com https://cdn.datatables.net; " +
            "style-src 'self' https://cdn.jsdelivr.net https://cdnjs.cloudflare.com https://fonts.googleapis.com https://cdn.datatables.net; " +
            "font-src 'self' https://cdn.jsdelivr.net https://fonts.gstatic.com https://cdnjs.cloudflare.com; " +
            "img-src 'self' data:; " +
            connectSrc;

        await _next(context);
    }

    private static string GenerateNonce()
    {
        var bytes = new byte[18];
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToBase64String(bytes);
    }
}
