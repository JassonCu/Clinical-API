namespace Clinical.Application.DTOS.Analysis.Response
{
    public class GetAnalysisResponseDto
    {
        public int AnalysisId { get; set; }
        public string? Name { get; set; }
        public DateTime AuditCreateDate { get; set; }
        public int State { get; set; }
        public string? StateAnalysis {  get; set; }
    }
}
