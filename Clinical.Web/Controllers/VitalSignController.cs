using Clinical.Web.Core.DTOs.VitalSign;
using Clinical.Web.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clinical.Web.Controllers;

public class VitalSignController : BaseController
{
    private readonly IVitalSignService _service;
    private readonly IPatientService _patients;

    public VitalSignController(IVitalSignService service, IPatientService patients)
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
        ViewBag.Patients = new SelectList(await _patients.GetAllAsync(), "PatientId", "FullName");
        return View(new CreateVitalSignDto { MeasuredAt = DateTime.Now });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateVitalSignDto model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Patients = new SelectList(await _patients.GetAllAsync(), "PatientId", "FullName");
            return View(model);
        }
        if (await _service.CreateAsync(model)) { SetSuccess("Signos vitales registrados."); return RedirectToAction(nameof(Index)); }
        SetError("No se pudo registrar.");
        ViewBag.Patients = new SelectList(await _patients.GetAllAsync(), "PatientId", "FullName");
        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        SetSuccess("Registro eliminado.");
        return RedirectToAction(nameof(Index));
    }
}
