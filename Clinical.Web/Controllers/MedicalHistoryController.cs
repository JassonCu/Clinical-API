using Clinical.Web.Core.DTOs.MedicalHistory;
using Clinical.Web.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clinical.Web.Controllers;

public class MedicalHistoryController : BaseController
{
    private readonly IMedicalHistoryService _service;
    private readonly IPatientService _patients;

    public MedicalHistoryController(IMedicalHistoryService service, IPatientService patients)
    {
        _service = service;
        _patients = patients;
    }

    public async Task<IActionResult> ByPatient(int patientId)
    {
        ViewBag.PatientId = patientId;
        return View(await _service.GetByPatientAsync(patientId));
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Patients = new SelectList(await _patients.GetAllAsync(), "PatientId", "FullName");
        return View(new CreateMedicalHistoryDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateMedicalHistoryDto model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Patients = new SelectList(await _patients.GetAllAsync(), "PatientId", "FullName");
            return View(model);
        }
        if (await _service.CreateAsync(model)) { SetSuccess("Historia clínica creada."); return RedirectToAction(nameof(ByPatient), new { patientId = model.PatientId }); }
        SetError("No se pudo crear.");
        ViewBag.Patients = new SelectList(await _patients.GetAllAsync(), "PatientId", "FullName");
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item is null) return NotFound();
        ViewBag.Patients = new SelectList(await _patients.GetAllAsync(), "PatientId", "FullName");
        return View(new UpdateMedicalHistoryDto
        {
            MedicalHistoryId = item.MedicalHistoryId, PatientId = item.PatientId,
            BloodType = item.BloodType, ChronicDiseases = item.ChronicDiseases,
            PreviousSurgeries = item.PreviousSurgeries, FamilyHistory = item.FamilyHistory,
            CurrentMedications = item.CurrentMedications, Habits = item.Habits, Observations = item.Observations
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UpdateMedicalHistoryDto model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Patients = new SelectList(await _patients.GetAllAsync(), "PatientId", "FullName");
            return View(model);
        }
        if (await _service.UpdateAsync(model)) { SetSuccess("Historia clínica actualizada."); return RedirectToAction(nameof(ByPatient), new { patientId = model.PatientId }); }
        SetError("No se pudo actualizar.");
        ViewBag.Patients = new SelectList(await _patients.GetAllAsync(), "PatientId", "FullName");
        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id, int patientId)
    {
        await _service.DeleteAsync(id);
        SetSuccess("Registro eliminado.");
        return RedirectToAction(nameof(ByPatient), new { patientId });
    }
}
