using Clinical.Web.Core.DTOs.Medicine;
using Clinical.Web.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinical.Web.Controllers;

public class MedicineController : BaseController
{
    private readonly IMedicineService _service;

    public MedicineController(IMedicineService service) => _service = service;

    public async Task<IActionResult> Index() => View(await _service.GetAllAsync());

    public async Task<IActionResult> Details(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item is null) return NotFound();
        return View(item);
    }

    public async Task<IActionResult> LowStock() => View(await _service.GetLowStockAsync());

    [HttpGet]
    [Authorize(Roles = "Admin,Pharmacist")]
    public IActionResult Create() => View(new CreateMedicineDto());

    [HttpPost]
    [Authorize(Roles = "Admin,Pharmacist")]
    public async Task<IActionResult> Create(CreateMedicineDto model)
    {
        if (!ModelState.IsValid) return View(model);
        if (await _service.CreateAsync(model)) { SetSuccess("Medicamento registrado."); return RedirectToAction(nameof(Index)); }
        SetError("No se pudo registrar.");
        return View(model);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Pharmacist")]
    public async Task<IActionResult> Edit(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item is null) return NotFound();
        return View(new UpdateMedicineDto
        {
            MedicineId = item.MedicineId, Code = item.Code, Name = item.Name,
            GenericName = item.GenericName, Brand = item.Brand, Category = item.Category,
            Presentation = item.Presentation, Concentration = item.Concentration, Unit = item.Unit,
            CurrentStock = item.CurrentStock, MinimumStock = item.MinimumStock, Price = item.Price,
            RequiresPrescription = item.RequiresPrescription, StorageConditions = item.StorageConditions,
            ExpirationDate = item.ExpirationDate
        });
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Pharmacist")]
    public async Task<IActionResult> Edit(UpdateMedicineDto model)
    {
        if (!ModelState.IsValid) return View(model);
        if (await _service.UpdateAsync(model)) { SetSuccess("Medicamento actualizado."); return RedirectToAction(nameof(Index)); }
        SetError("No se pudo actualizar.");
        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        SetSuccess("Medicamento eliminado.");
        return RedirectToAction(nameof(Index));
    }
}
