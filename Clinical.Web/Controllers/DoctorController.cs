using Clinical.Web.Core.DTOs.Doctor;
using Clinical.Web.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Clinical.Web.Controllers;

public class DoctorController : BaseController
{
    private readonly IDoctorService _service;

    public DoctorController(IDoctorService service) => _service = service;

    public async Task<IActionResult> Index() => View(await _service.GetAllAsync());

    public async Task<IActionResult> Details(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item is null) return NotFound();
        return View(item);
    }

    [HttpGet]
    public IActionResult Create() => View(new CreateDoctorDto());

    [HttpPost]
    public async Task<IActionResult> Create(CreateDoctorDto model)
    {
        if (!ModelState.IsValid) return View(model);
        if (await _service.CreateAsync(model)) { SetSuccess("Médico registrado."); return RedirectToAction(nameof(Index)); }
        SetError("No se pudo registrar el médico.");
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item is null) return NotFound();
        return View(new UpdateDoctorDto
        {
            DoctorId = item.DoctorId, DocumentNumber = item.DocumentNumber,
            FirstName = item.FirstName, LastName = item.LastName,
            Specialty = item.Specialty, Email = item.Email,
            Phone = item.Phone, MedicalLicense = item.MedicalLicense
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UpdateDoctorDto model)
    {
        if (!ModelState.IsValid) return View(model);
        if (await _service.UpdateAsync(model)) { SetSuccess("Médico actualizado."); return RedirectToAction(nameof(Index)); }
        SetError("No se pudo actualizar.");
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        SetSuccess("Médico eliminado.");
        return RedirectToAction(nameof(Index));
    }
}
