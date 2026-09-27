using System.Security.Claims;
using Clinical.Interface.Interfaces;

namespace Clinical.API.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _accessor;

        public CurrentUserService(IHttpContextAccessor accessor) => _accessor = accessor;

        private HttpContext? Context => _accessor.HttpContext;

        public int? UserId =>
            int.TryParse(Context?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Context?.User.FindFirstValue("sub"),
                out var id) ? id : null;

        public string? UserName =>
            Context?.User.Identity?.IsAuthenticated == true ? Context.User.Identity.Name : null;

        public string? IpAddress => Context?.Connection.RemoteIpAddress?.ToString();

        public string? TraceId => Context?.TraceIdentifier;
    }
}
