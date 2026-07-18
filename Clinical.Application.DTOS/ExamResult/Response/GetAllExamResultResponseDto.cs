namespace Clinical.Application.DTOS.ExamResult.Response
{
    public class GetAllExamResultResponseDto
    {
        public int ExamResultId { get; set; }
        public string? PatientFullName { get; set; }
        public string? ExamName { get; set; }
        public string? AnalysisName { get; set; }
        public string? Result { get; set; }
        public DateTime ResultDate { get; set; }
        public int State { get; set; }
        public string? StateResult => State == 1 ? "COMPLETADO" : "PENDIENTE";
        public DateTime AuditCreateDate { get; set; }
    }
}
