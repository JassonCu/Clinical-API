using Clinical.Web.Core.DTOs.Patient;
using Clinical.Web.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Clinical.Web.Controllers;

public class PatientController : BaseController
{
    private readonly IPatientService _service;

    public PatientController(IPatientService service) => _service = service;

    public async Task<IActionResult> Index()
        => View(await _service.GetAllAsync());

    public async Task<IActionResult> Details(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item is null) return NotFound();
        return View(item);
    }

    [HttpGet]
    public IActionResult Create() => View(new CreatePatientDto());

    [HttpPost]
    public async Task<IActionResult> Create(CreatePatientDto model)
    {
        if (!ModelState.IsValid) return View(model);
        if (await _service.CreateAsync(model)) { SetSuccess("Paciente registrado correctamente."); return RedirectToAction(nameof(Index)); }
        SetError("No se pudo registrar el paciente.");
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item is null) return NotFound();
        return View(new UpdatePatientDto
        {
            PatientId = item.PatientId, DocumentNumber = item.DocumentNumber,
            FirstName = item.FirstName, LastName = item.LastName,
            Email = item.Email, Phone = item.Phone,
            BirthDate = item.BirthDate, Gender = item.Gender, Address = item.Address
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UpdatePatientDto model)
    {
        if (!ModelState.IsValid) return View(model);
        if (await _service.UpdateAsync(model)) { SetSuccess("Paciente actualizado."); return RedirectToAction(nameof(Index)); }
        SetError("No se pudo actualizar el paciente.");
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        SetSuccess("Paciente eliminado.");
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> ChangeState(int id, int state)
    {
        await _service.ChangeStateAsync(new ChangeStatePatientDto { PatientId = id, State = state });
        return RedirectToAction(nameof(Index));
    }
}
