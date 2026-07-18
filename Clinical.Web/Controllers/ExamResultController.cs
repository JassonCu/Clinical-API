using Clinical.Web.Core.DTOs.ExamResult;
using Clinical.Web.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clinical.Web.Controllers;

public class ExamResultController : BaseController
{
    private readonly IExamResultService _service;
    private readonly IPatientService _patients;
    private readonly IExamService _exams;

    public ExamResultController(IExamResultService service, IPatientService patients, IExamService exams)
    {
        _service = service; _patients = patients; _exams = exams;
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
        return View(new CreateExamResultDto { ResultDate = DateTime.Today });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateExamResultDto model)
    {
        if (!ModelState.IsValid) { await PopulateDropdowns(); return View(model); }
        if (await _service.CreateAsync(model)) { SetSuccess("Resultado registrado."); return RedirectToAction(nameof(Index)); }
        SetError("No se pudo registrar.");
        await PopulateDropdowns();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        SetSuccess("Resultado eliminado.");
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateDropdowns()
    {
        ViewBag.Patients = new SelectList(await _patients.GetAllAsync(), "PatientId", "FullName");
        ViewBag.Exams = new SelectList(await _exams.GetAllAsync(), "ExamId", "Name");
    }
}
