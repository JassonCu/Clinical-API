namespace Clinical.Domain.Entities
{
    public class Prescription
    {
        public int? PrescriptionId { get; set; }
        public int? PatientId { get; set; }
        public int? DoctorId { get; set; }
        public int? AppointmentId { get; set; }
        public DateTime? PrescriptionDate { get; set; }
        public DateTime? ValidUntil { get; set; }
        public string? Notes { get; set; }
        public int? State { get; set; }
        public DateTime? AuditCreateDate { get; set; }
    }
}
