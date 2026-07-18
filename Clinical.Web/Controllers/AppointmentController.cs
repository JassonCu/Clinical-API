using Clinical.Web.Core.DTOs.Appointment;
using Clinical.Web.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clinical.Web.Controllers;

public class AppointmentController : BaseController
{
    private readonly IAppointmentService _service;
    private readonly IPatientService _patients;
    private readonly IDoctorService _doctors;

    public AppointmentController(IAppointmentService service, IPatientService patients, IDoctorService doctors)
    {
        _service = service;
        _patients = patients;
        _doctors = doctors;
    }

    public async Task<IActionResult> Index() => View(await _service.GetAllAsync());

    public async Task<IActionResult> Details(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item is null) return NotFound();
        return View(item);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await PopulateDropdowns();
        return View(new CreateAppointmentDto { AppointmentDate = DateTime.Now.AddHours(1) });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAppointmentDto model)
    {
        if (!ModelState.IsValid) { await PopulateDropdowns(); return View(model); }
        if (await _service.CreateAsync(model)) { SetSuccess("Cita registrada."); return RedirectToAction(nameof(Index)); }
        SetError("No se pudo registrar la cita.");
        await PopulateDropdowns();
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item is null) return NotFound();
        await PopulateDropdowns();
        return View(new UpdateAppointmentDto
        {
            AppointmentId = item.AppointmentId, PatientId = item.PatientId,
            DoctorId = item.DoctorId, AppointmentDate = item.AppointmentDate,
            Reason = item.Reason, Diagnosis = item.Diagnosis, Notes = item.Notes
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UpdateAppointmentDto model)
    {
        if (!ModelState.IsValid) { await PopulateDropdowns(); return View(model); }
        if (await _service.UpdateAsync(model)) { SetSuccess("Cita actualizada."); return RedirectToAction(nameof(Index)); }
        SetError("No se pudo actualizar.");
        await PopulateDropdowns();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        SetSuccess("Cita eliminada.");
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> ChangeState(int id, int state)
    {
        await _service.ChangeStateAsync(new ChangeStateAppointmentDto { AppointmentId = id, State = state });
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateDropdowns()
    {
        var patients = await _patients.GetAllAsync();
        var doctors = await _doctors.GetAllAsync();
        ViewBag.Patients = new SelectList(patients, "PatientId", "FullName");
        ViewBag.Doctors = new SelectList(doctors, "DoctorId", "FullName");
    }
}
