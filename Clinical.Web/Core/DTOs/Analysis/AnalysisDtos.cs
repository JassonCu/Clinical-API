using System.ComponentModel.DataAnnotations;

namespace Clinical.Web.Core.DTOs.Analysis;

public class AnalysisListDto
{
    public int AnalysisId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime AuditCreateDate { get; set; }
    public int State { get; set; }
    public string StateAnalysis { get; set; } = string.Empty;
}

public class CreateAnalysisDto
{
    [Required][Display(Name = "Nombre del análisis")] public string Name { get; set; } = string.Empty;
}

public class UpdateAnalysisDto : CreateAnalysisDto
{
    public int AnalysisId { get; set; }
}

public class ChangeStateAnalysisDto
{
    public int AnalysisId { get; set; }
    public int State { get; set; }
}
