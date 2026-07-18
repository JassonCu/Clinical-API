namespace Clinical.Domain.Entities
{
    public class Appointment
    {
        public int? AppointmentId { get; set; }
        public int? PatientId { get; set; }
        public int? DoctorId { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string? Reason { get; set; }
        public string? Diagnosis { get; set; }
        public string? Notes { get; set; }
        public int? State { get; set; }
        public DateTime? AuditCreateDate { get; set; }
    }
}
