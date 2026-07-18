using Clinical.Web.Core.DTOs.Prescription;
using Clinical.Web.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clinical.Web.Controllers;

public class PrescriptionController : BaseController
{
    private readonly IPrescriptionService _service;
    private readonly IPatientService _patients;
    private readonly IDoctorService _doctors;
    private readonly IMedicineService _medicines;
    private readonly IAppointmentService _appointments;

    public PrescriptionController(IPrescriptionService service, IPatientService patients,
        IDoctorService doctors, IMedicineService medicines, IAppointmentService appointments)
    {
        _service = service; _patients = patients; _doctors = doctors;
        _medicines = medicines; _appointments = appointments;
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
        return View(new CreatePrescriptionDto { PrescriptionDate = DateTime.Today });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePrescriptionDto model)
    {
        if (!ModelState.IsValid) { await PopulateDropdowns(); return View(model); }
        if (await _service.CreateAsync(model)) { SetSuccess("Receta creada."); return RedirectToAction(nameof(Index)); }
        SetError("No se pudo crear la receta.");
        await PopulateDropdowns();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        SetSuccess("Receta eliminada.");
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateDropdowns()
    {
        ViewBag.Patients = new SelectList(await _patients.GetAllAsync(), "PatientId", "FullName");
        ViewBag.Doctors = new SelectList(await _doctors.GetAllAsync(), "DoctorId", "FullName");
        ViewBag.Medicines = await _medicines.GetAllAsync();
    }
}
