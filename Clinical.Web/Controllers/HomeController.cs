using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinical.Web.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{
    public IActionResult Error() => View();
}
