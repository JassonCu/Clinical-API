using Clinical.Web.Core.DTOs.Exam;
using Clinical.Web.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clinical.Web.Controllers;

public class ExamController : BaseController
{
    private readonly IExamService _service;
    private readonly IAnalysisService _analyses;

    public ExamController(IExamService service, IAnalysisService analyses)
    {
        _service = service;
        _analyses = analyses;
    }

    public async Task<IActionResult> Index() => View(await _service.GetAllAsync());

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Analyses = new SelectList(await _analyses.GetAllAsync(), "AnalysisId", "Name");
        return View(new CreateExamDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateExamDto model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Analyses = new SelectList(await _analyses.GetAllAsync(), "AnalysisId", "Name");
            return View(model);
        }
        if (await _service.CreateAsync(model)) { SetSuccess("Examen registrado."); return RedirectToAction(nameof(Index)); }
        SetError("No se pudo registrar.");
        ViewBag.Analyses = new SelectList(await _analyses.GetAllAsync(), "AnalysisId", "Name");
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        SetSuccess("Examen eliminado.");
        return RedirectToAction(nameof(Index));
    }
}
