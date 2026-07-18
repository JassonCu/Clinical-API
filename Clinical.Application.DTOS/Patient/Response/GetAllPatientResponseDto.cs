namespace Clinical.Application.DTOS.Patient.Response
{
    public class GetAllPatientResponseDto
    {
        public int PatientId { get; set; }
        public string? DocumentNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName => $"{FirstName} {LastName}";
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public int State { get; set; }
        public string? StatePatient => State == 1 ? "ACTIVO" : "INACTIVO";
        public DateTime AuditCreateDate { get; set; }
    }
}
