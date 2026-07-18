using Clinical.Web.Core.DTOs.Auth;
using Clinical.Web.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinical.Web.Controllers;

[AllowAnonymous]
public class SetupController : Controller
{
    private readonly ISetupService _setupService;

    public SetupController(ISetupService setupService) => _setupService = setupService;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (await _setupService.IsInitializedAsync())
            return RedirectToAction("Login", "Auth");
        return View(new SetupInitDto());
    }

    [HttpPost]
    public async Task<IActionResult> Index(SetupInitDto model)
    {
        if (!ModelState.IsValid) return View(model);

        var (success, error) = await _setupService.InitAsync(model);
        if (success)
        {
            TempData["Success"] = "Sistema inicializado correctamente. Por favor inicie sesión.";
            return RedirectToAction("Login", "Auth");
        }
        ModelState.AddModelError(string.Empty, error ?? "Error al inicializar el sistema.");
        return View(model);
    }
}
