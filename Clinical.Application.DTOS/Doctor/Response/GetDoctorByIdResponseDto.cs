namespace Clinical.Application.DTOS.Doctor.Response
{
    public class GetDoctorByIdResponseDto
    {
        public int DoctorId { get; set; }
        public string? DocumentNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Specialty { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? MedicalLicense { get; set; }
        public int? State { get; set; }
        public DateTime? AuditCreateDate { get; set; }
    }
}
