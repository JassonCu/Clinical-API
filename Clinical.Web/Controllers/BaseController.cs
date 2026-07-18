using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Clinical.Web.Controllers;

[Authorize]
public abstract class BaseController : Controller
{
    protected string CurrentUser => User.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
    protected string CurrentFullName => User.FindFirstValue("full_name") ?? string.Empty;
    protected string CurrentRole => User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
    protected bool IsAdmin => CurrentRole.Equals("Admin", StringComparison.OrdinalIgnoreCase);
    protected int CurrentUserId => int.TryParse(User.FindFirstValue("user_id"), out var id) ? id : 0;

    protected void SetSuccess(string message) => TempData["Success"] = message;
    protected void SetError(string message) => TempData["Error"] = message;

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);
        if (User.Identity?.IsAuthenticated == true &&
            User.FindFirstValue("must_change_password") == "true")
        {
            context.Result = RedirectToAction("ChangePassword", "Auth");
        }
    }
}
