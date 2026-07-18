namespace Clinical.Domain.Entities
{
    public class PatientDiagnosis
    {
        public int? DiagnosisId { get; set; }
        public int? AppointmentId { get; set; }
        public int? PatientId { get; set; }
        public string? IcdCode { get; set; }
        public string? Description { get; set; }
        public string? DiagnosisType { get; set; }
        public string? Notes { get; set; }
        public int? State { get; set; }
        public DateTime? AuditCreateDate { get; set; }
        public string? PatientFullName { get; set; }
    }
}
