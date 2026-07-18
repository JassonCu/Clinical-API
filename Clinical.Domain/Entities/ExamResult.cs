namespace Clinical.Domain.Entities
{
    public class ExamResult
    {
        public int? ExamResultId { get; set; }
        public int? PatientId { get; set; }
        public int? ExamId { get; set; }
        public int? AppointmentId { get; set; }
        public string? Result { get; set; }
        public string? Observations { get; set; }
        public DateTime? ResultDate { get; set; }
        public int? State { get; set; }
        public DateTime? AuditCreateDate { get; set; }
    }
}
