namespace Clinical.Application.DTOS.ExamResult.Response
{
    public class GetExamResultByIdResponseDto
    {
        public int ExamResultId { get; set; }
        public int PatientId { get; set; }
        public string? PatientFullName { get; set; }
        public int ExamId { get; set; }
        public string? ExamName { get; set; }
        public string? AnalysisName { get; set; }
        public int? AppointmentId { get; set; }
        public string? Result { get; set; }
        public string? Observations { get; set; }
        public DateTime ResultDate { get; set; }
        public int? State { get; set; }
        public DateTime? AuditCreateDate { get; set; }
    }
}
