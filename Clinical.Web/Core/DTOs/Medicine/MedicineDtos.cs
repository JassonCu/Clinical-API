using System.ComponentModel.DataAnnotations;

namespace Clinical.Web.Core.DTOs.Medicine;

public class MedicineListDto
{
    public int MedicineId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string GenericName { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Presentation { get; set; } = string.Empty;
    public string Concentration { get; set; } = string.Empty;
    public int CurrentStock { get; set; }
    public int MinimumStock { get; set; }
    public decimal Price { get; set; }
    public bool RequiresPrescription { get; set; }
    public bool IsLowStock { get; set; }
    public int State { get; set; }
    public string StateMedicine { get; set; } = string.Empty;
}

public class MedicineDetailDto
{
    public int MedicineId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string GenericName { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Presentation { get; set; } = string.Empty;
    public string Concentration { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public int CurrentStock { get; set; }
    public int MinimumStock { get; set; }
    public decimal Price { get; set; }
    public bool RequiresPrescription { get; set; }
    public string? StorageConditions { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public int State { get; set; }
    public DateTime AuditCreateDate { get; set; }
}

public class CreateMedicineDto
{
    [Required][Display(Name = "Código")] public string Code { get; set; } = string.Empty;
    [Required][Display(Name = "Nombre comercial")] public string Name { get; set; } = string.Empty;
    [Display(Name = "Nombre genérico")] public string? GenericName { get; set; }
    [Display(Name = "Marca")] public string? Brand { get; set; }
    [Display(Name = "Categoría")] public string? Category { get; set; }
    [Display(Name = "Presentación")] public string? Presentation { get; set; }
    [Display(Name = "Concentración")] public string? Concentration { get; set; }
    [Display(Name = "Unidad")] public string? Unit { get; set; }
    [Required][Range(0, int.MaxValue)][Display(Name = "Stock actual")] public int CurrentStock { get; set; }
    [Required][Range(0, int.MaxValue)][Display(Name = "Stock mínimo")] public int MinimumStock { get; set; }
    [Required][Range(0, double.MaxValue)][Display(Name = "Precio")] public decimal Price { get; set; }
    [Display(Name = "Requiere receta")] public bool RequiresPrescription { get; set; }
    [Display(Name = "Condiciones de almacenamiento")] public string? StorageConditions { get; set; }
    [Display(Name = "Fecha de vencimiento")] public DateTime? ExpirationDate { get; set; }
}

public class UpdateMedicineDto : CreateMedicineDto
{
    public int MedicineId { get; set; }
}

public class ChangeStateMedicineDto
{
    public int MedicineId { get; set; }
    public int State { get; set; }
}
