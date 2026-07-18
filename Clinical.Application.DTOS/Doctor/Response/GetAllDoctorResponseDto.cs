namespace Clinical.Application.DTOS.Doctor.Response
{
    public class GetAllDoctorResponseDto
    {
        public int DoctorId { get; set; }
        public string? DocumentNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName => $"{FirstName} {LastName}";
        public string? Specialty { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int State { get; set; }
        public string? StateDoctor => State == 1 ? "ACTIVO" : "INACTIVO";
        public DateTime AuditCreateDate { get; set; }
    }
}
