using System.ComponentModel.DataAnnotations;

namespace Clinical.Web.Core.DTOs.Exam;

public class ExamListDto
{
    public int ExamId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Analysis { get; set; } = string.Empty;
    public DateTime AuditCreateDate { get; set; }
    public string StateExam { get; set; } = string.Empty;
}

public class ExamDetailDto
{
    public int ExamId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int AnalysisId { get; set; }
}

public class CreateExamDto
{
    [Required][Display(Name = "Nombre del examen")] public string Name { get; set; } = string.Empty;
    [Required][Display(Name = "Análisis")] public int AnalysisId { get; set; }
}

public class UpdateExamDto : CreateExamDto
{
    public int ExamId { get; set; }
}

public class ChangeStateExamDto
{
    public int ExamId { get; set; }
    public int State { get; set; }
}
