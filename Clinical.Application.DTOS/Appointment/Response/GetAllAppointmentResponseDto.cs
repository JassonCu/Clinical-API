namespace Clinical.Application.DTOS.Appointment.Response
{
    public class GetAllAppointmentResponseDto
    {
        public int AppointmentId { get; set; }
        public string? PatientFullName { get; set; }
        public string? DoctorFullName { get; set; }
        public string? DoctorSpecialty { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string? Reason { get; set; }
        public int State { get; set; }
        public string? StateAppointment => State == 1 ? "ACTIVO" : "CANCELADO";
        public DateTime AuditCreateDate { get; set; }
    }
}
