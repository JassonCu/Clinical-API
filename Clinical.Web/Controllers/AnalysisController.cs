using Clinical.Web.Core.DTOs.Analysis;
using Clinical.Web.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Clinical.Web.Controllers;

public class AnalysisController : BaseController
{
    private readonly IAnalysisService _service;

    public AnalysisController(IAnalysisService service) => _service = service;

    public async Task<IActionResult> Index() => View(await _service.GetAllAsync());

    [HttpGet]
    public IActionResult Create() => View(new CreateAnalysisDto());

    [HttpPost]
    public async Task<IActionResult> Create(CreateAnalysisDto model)
    {
        if (!ModelState.IsValid) return View(model);
        if (await _service.CreateAsync(model)) { SetSuccess("Análisis registrado."); return RedirectToAction(nameof(Index)); }
        SetError("No se pudo registrar.");
        return View(model);
    }

    [HttpGet]
    public IActionResult Edit(int id) => View(new UpdateAnalysisDto { AnalysisId = id });

    [HttpPost]
    public async Task<IActionResult> Edit(UpdateAnalysisDto model)
    {
        if (!ModelState.IsValid) return View(model);
        if (await _service.UpdateAsync(model)) { SetSuccess("Análisis actualizado."); return RedirectToAction(nameof(Index)); }
        SetError("No se pudo actualizar.");
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        SetSuccess("Análisis eliminado.");
        return RedirectToAction(nameof(Index));
    }
}
