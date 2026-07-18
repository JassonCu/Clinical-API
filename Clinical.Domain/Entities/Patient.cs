namespace Clinical.Domain.Entities
{
    public class Patient
    {
        public int? PatientId { get; set; }
        public string? DocumentType { get; set; }       // DNI, Passport, CE
        public string? DocumentNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? MobilePhone { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Gender { get; set; }             // M, F, O
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? BloodType { get; set; }          // A+, A-, B+, B-, AB+, AB-, O+, O-
        public string? InsuranceCompany { get; set; }
        public string? InsuranceNumber { get; set; }
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContactPhone { get; set; }
        public string? Nationality { get; set; }
        public string? MaritalStatus { get; set; }      // single, married, divorced, widowed
        public string? Occupation { get; set; }
        public int? State { get; set; }
        public DateTime? AuditCreateDate { get; set; }
    }
}
