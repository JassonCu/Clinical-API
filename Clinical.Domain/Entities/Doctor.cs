namespace Clinical.Domain.Entities
{
    public class Doctor
    {
        public int? DoctorId { get; set; }
        public string? DocumentNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Specialty { get; set; }
        public string? Subspecialty { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? MobilePhone { get; set; }
        public string? MedicalLicense { get; set; }
        public decimal? ConsultationFee { get; set; }
        public string? WorkingSchedule { get; set; }
        public string? Biography { get; set; }
        public int? State { get; set; }
        public DateTime? AuditCreateDate { get; set; }
    }
}
