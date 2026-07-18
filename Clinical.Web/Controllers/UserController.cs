using Clinical.Web.Core.DTOs.User;
using Clinical.Web.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clinical.Web.Controllers;

[Authorize(Roles = "Admin")]
public class UserController : BaseController
{
    private readonly IUserService _service;

    public UserController(IUserService service) => _service = service;

    public async Task<IActionResult> Index()
        => View(await _service.GetAllAsync());

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await PopulateRolesAsync();
        return View(new CreateUserDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserDto model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateRolesAsync();
            return View(model);
        }

        var (ok, error) = await _service.CreateAsync(model);
        if (ok) { SetSuccess("Usuario creado correctamente."); return RedirectToAction(nameof(Index)); }

        SetError(error ?? "No se pudo crear el usuario.");
        await PopulateRolesAsync();
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item is null) return NotFound();

        await PopulateRolesAsync();
        return View(new UpdateUserDto
        {
            UserId = item.UserId,
            FirstName = item.FirstName,
            LastName = item.LastName,
            Email = item.Email,
            RoleId = item.RoleId
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UpdateUserDto model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateRolesAsync();
            return View(model);
        }

        if (await _service.UpdateAsync(model)) { SetSuccess("Usuario actualizado correctamente."); return RedirectToAction(nameof(Index)); }

        SetError("No se pudo actualizar el usuario.");
        await PopulateRolesAsync();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ChangeState(int id, int state)
    {
        await _service.ChangeStateAsync(new ChangeStateUserDto { UserId = id, State = state });
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> GenerateResetToken(int id)
    {
        var result = await _service.GenerateResetTokenAsync(id);
        if (result is null)
        {
            SetError("No se pudo generar el token de recuperación.");
            return RedirectToAction(nameof(Index));
        }
        TempData["ResetToken"] = result.RawToken;
        TempData["ResetTokenUser"] = result.TargetUsername;
        TempData["ResetTokenExpiry"] = result.ExpiresAt.ToString("dd/MM/yyyy HH:mm");
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateRolesAsync()
    {
        var roles = await _service.GetRolesAsync();
        ViewBag.Roles = new SelectList(roles, "RoleId", "Name");
    }
}
