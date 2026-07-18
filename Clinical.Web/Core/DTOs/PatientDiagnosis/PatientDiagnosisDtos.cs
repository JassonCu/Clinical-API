using System.ComponentModel.DataAnnotations;

namespace Clinical.Web.Core.DTOs.PatientDiagnosis;

public class DiagnosisDto
{
    public int DiagnosisId { get; set; }
    public int AppointmentId { get; set; }
    public int PatientId { get; set; }
    public string PatientFullName { get; set; } = string.Empty;
    public string IcdCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? DiagnosisType { get; set; }
    public string? Notes { get; set; }
    public int State { get; set; }
    public DateTime AuditCreateDate { get; set; }
}

public class CreateDiagnosisDto
{
    [Required][Display(Name = "Paciente")] public int PatientId { get; set; }
    [Required][Display(Name = "Cita")] public int AppointmentId { get; set; }
    [Required][MaxLength(20)][Display(Name = "Código CIE-10")] public string IcdCode { get; set; } = string.Empty;
    [Required][Display(Name = "Descripción")] public string Description { get; set; } = string.Empty;
    [Display(Name = "Tipo de diagnóstico")] public string? DiagnosisType { get; set; }
    [Display(Name = "Notas")] public string? Notes { get; set; }
}

public class ChangeStateDiagnosisDto
{
    public int DiagnosisId { get; set; }
    public int State { get; set; }
}
