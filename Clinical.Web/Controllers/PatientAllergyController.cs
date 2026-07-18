using Clinical.Web.Core.DTOs.PatientAllergy;
using Clinical.Web.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clinical.Web.Controllers;

public class PatientAllergyController : BaseController
{
    private readonly IPatientAllergyService _service;
    private readonly IPatientService _patients;

    public PatientAllergyController(IPatientAllergyService service, IPatientService patients)
    {
        _service = service;
        _patients = patients;
    }

    public async Task<IActionResult> Index() => View(await _service.GetAllAsync());

    public async Task<IActionResult> ByPatient(int patientId)
    {
        ViewBag.PatientId = patientId;
        return View(await _service.GetByPatientAsync(patientId));
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        PopulateSeverity();
        ViewBag.Patients = new SelectList(await _patients.GetAllAsync(), "PatientId", "FullName");
        return View(new CreateAllergyDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAllergyDto model)
    {
        if (!ModelState.IsValid) { await FillViewBag(); return View(model); }
        if (await _service.CreateAsync(model)) { SetSuccess("Alergia registrada."); return RedirectToAction(nameof(Index)); }
        SetError("No se pudo registrar.");
        await FillViewBag();
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item is null) return NotFound();
        await FillViewBag();
        return View(new UpdateAllergyDto
        {
            AllergyId = item.AllergyId, PatientId = item.PatientId,
            AllergenType = item.AllergenType, AllergenName = item.AllergenName,
            Reaction = item.Reaction, Severity = item.Severity, Notes = item.Notes
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UpdateAllergyDto model)
    {
        if (!ModelState.IsValid) { await FillViewBag(); return View(model); }
        if (await _service.UpdateAsync(model)) { SetSuccess("Alergia actualizada."); return RedirectToAction(nameof(Index)); }
        SetError("No se pudo actualizar.");
        await FillViewBag();
        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        SetSuccess("Alergia eliminada.");
        return RedirectToAction(nameof(Index));
    }

    private void PopulateSeverity() =>
        ViewBag.Severities = new SelectList(new[] { "Leve", "Moderada", "Grave", "Anafilaxia" });

    private async Task FillViewBag()
    {
        PopulateSeverity();
        ViewBag.Patients = new SelectList(await _patients.GetAllAsync(), "PatientId", "FullName");
    }
}
