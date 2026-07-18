namespace Clinical.Domain.Entities
{
    public class VitalSign
    {
        public int? VitalSignId { get; set; }
        public int? PatientId { get; set; }
        public int? AppointmentId { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
        public decimal? Bmi { get; set; }
        public int? BloodPressureSystolic { get; set; }
        public int? BloodPressureDiastolic { get; set; }
        public int? HeartRate { get; set; }
        public decimal? Temperature { get; set; }
        public int? OxygenSaturation { get; set; }
        public int? RespiratoryRate { get; set; }
        public decimal? GlucoseLevel { get; set; }
        public string? Notes { get; set; }
        public DateTime? MeasuredAt { get; set; }
        public int? State { get; set; }
        public DateTime? AuditCreateDate { get; set; }
        public string? PatientFullName { get; set; }
    }
}
