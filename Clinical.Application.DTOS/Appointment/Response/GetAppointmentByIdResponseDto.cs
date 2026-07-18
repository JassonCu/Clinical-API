namespace Clinical.Application.DTOS.Appointment.Response
{
    public class GetAppointmentByIdResponseDto
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public string? PatientFullName { get; set; }
        public int DoctorId { get; set; }
        public string? DoctorFullName { get; set; }
        public string? DoctorSpecialty { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string? Reason { get; set; }
        public string? Diagnosis { get; set; }
        public string? Notes { get; set; }
        public int? State { get; set; }
        public DateTime? AuditCreateDate { get; set; }
    }
}
