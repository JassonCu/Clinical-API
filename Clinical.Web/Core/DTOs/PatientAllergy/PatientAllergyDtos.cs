using System.ComponentModel.DataAnnotations;

namespace Clinical.Web.Core.DTOs.PatientAllergy;

public class AllergyDto
{
    public int AllergyId { get; set; }
    public int PatientId { get; set; }
    public string PatientFullName { get; set; } = string.Empty;
    public string AllergenType { get; set; } = string.Empty;
    public string AllergenName { get; set; } = string.Empty;
    public string? Reaction { get; set; }
    public string Severity { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public int State { get; set; }
    public DateTime AuditCreateDate { get; set; }
}

public class CreateAllergyDto
{
    [Required][Display(Name = "Paciente")] public int PatientId { get; set; }
    [Required][Display(Name = "Tipo de alérgeno")] public string AllergenType { get; set; } = string.Empty;
    [Required][Display(Name = "Alérgeno")] public string AllergenName { get; set; } = string.Empty;
    [Display(Name = "Reacción")] public string? Reaction { get; set; }
    [Required][Display(Name = "Severidad")] public string Severity { get; set; } = string.Empty;
    [Display(Name = "Notas")] public string? Notes { get; set; }
}

public class UpdateAllergyDto : CreateAllergyDto
{
    public int AllergyId { get; set; }
}

public class ChangeStateAllergyDto
{
    public int AllergyId { get; set; }
    public int State { get; set; }
}
