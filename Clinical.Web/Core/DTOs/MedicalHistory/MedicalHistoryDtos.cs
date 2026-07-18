using System.ComponentModel.DataAnnotations;

namespace Clinical.Web.Core.DTOs.MedicalHistory;

public class MedicalHistoryDto
{
    public int MedicalHistoryId { get; set; }
    public int PatientId { get; set; }
    public string PatientFullName { get; set; } = string.Empty;
    public string? BloodType { get; set; }
    public string? ChronicDiseases { get; set; }
    public string? PreviousSurgeries { get; set; }
    public string? FamilyHistory { get; set; }
    public string? CurrentMedications { get; set; }
    public string? Habits { get; set; }
    public string? Observations { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
    public int State { get; set; }
    public DateTime AuditCreateDate { get; set; }
}

public class CreateMedicalHistoryDto
{
    [Required][Display(Name = "Paciente")] public int PatientId { get; set; }
    [Display(Name = "Grupo Sanguíneo")] public string? BloodType { get; set; }
    [Display(Name = "Enfermedades crónicas")] public string? ChronicDiseases { get; set; }
    [Display(Name = "Cirugías previas")] public string? PreviousSurgeries { get; set; }
    [Display(Name = "Antecedentes familiares")] public string? FamilyHistory { get; set; }
    [Display(Name = "Medicamentos actuales")] public string? CurrentMedications { get; set; }
    [Display(Name = "Hábitos")] public string? Habits { get; set; }
    [Display(Name = "Observaciones")] public string? Observations { get; set; }
}

public class UpdateMedicalHistoryDto : CreateMedicalHistoryDto
{
    public int MedicalHistoryId { get; set; }
}
