using Clinical.Web.Core.DTOs.PatientDiagnosis;
using Clinical.Web.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clinical.Web.Controllers;

public class PatientDiagnosisController : BaseController
{
    private readonly IPatientDiagnosisService _service;
    private readonly IPatientService _patients;
    private readonly IAppointmentService _appointments;

    public PatientDiagnosisController(IPatientDiagnosisService service, IPatientService patients, IAppointmentService appointments)
    {
        _service = service; _patients = patients; _appointments = appointments;
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
        await PopulateDropdowns();
        return View(new CreateDiagnosisDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateDiagnosisDto model)
    {
        if (!ModelState.IsValid) { await PopulateDropdowns(); return View(model); }
        if (await _service.CreateAsync(model)) { SetSuccess("Diagnóstico registrado."); return RedirectToAction(nameof(Index)); }
        SetError("No se pudo registrar.");
        await PopulateDropdowns();
        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        SetSuccess("Diagnóstico eliminado.");
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateDropdowns()
    {
        ViewBag.Patients = new SelectList(await _patients.GetAllAsync(), "PatientId", "FullName");
        ViewBag.Appointments = new SelectList(await _appointments.GetAllAsync(), "AppointmentId", "AppointmentId");
    }
}
