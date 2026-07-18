using System.ComponentModel.DataAnnotations;

namespace Clinical.Web.Core.DTOs.ExamResult;

public class ExamResultListDto
{
    public int ExamResultId { get; set; }
    public string PatientFullName { get; set; } = string.Empty;
    public string ExamName { get; set; } = string.Empty;
    public string AnalysisName { get; set; } = string.Empty;
    public string Result { get; set; } = string.Empty;
    public DateTime ResultDate { get; set; }
    public int State { get; set; }
    public string StateResult { get; set; } = string.Empty;
    public DateTime AuditCreateDate { get; set; }
}

public class ExamResultDetailDto
{
    public int ExamResultId { get; set; }
    public int PatientId { get; set; }
    public string PatientFullName { get; set; } = string.Empty;
    public int ExamId { get; set; }
    public string ExamName { get; set; } = string.Empty;
    public string AnalysisName { get; set; } = string.Empty;
    public int? AppointmentId { get; set; }
    public string Result { get; set; } = string.Empty;
    public string? Observations { get; set; }
    public DateTime ResultDate { get; set; }
    public int State { get; set; }
    public DateTime AuditCreateDate { get; set; }
}

public class CreateExamResultDto
{
    [Required][Display(Name = "Paciente")] public int PatientId { get; set; }
    [Required][Display(Name = "Examen")] public int ExamId { get; set; }
    [Display(Name = "Cita")] public int? AppointmentId { get; set; }
    [Required][Display(Name = "Resultado")] public string Result { get; set; } = string.Empty;
    [Display(Name = "Observaciones")] public string? Observations { get; set; }
    [Required][Display(Name = "Fecha del resultado")] public DateTime ResultDate { get; set; } = DateTime.Today;
}

public class UpdateExamResultDto : CreateExamResultDto
{
    public int ExamResultId { get; set; }
}

public class ChangeStateExamResultDto
{
    public int ExamResultId { get; set; }
    public int State { get; set; }
}
