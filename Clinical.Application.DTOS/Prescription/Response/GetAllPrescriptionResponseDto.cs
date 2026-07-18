namespace Clinical.Application.DTOS.Prescription.Response
{
    public class GetAllPrescriptionResponseDto
    {
        public int PrescriptionId { get; set; }
        public string? PatientFullName { get; set; }
        public string? DoctorFullName { get; set; }
        public int? AppointmentId { get; set; }
        public DateTime? PrescriptionDate { get; set; }
        public DateTime? ValidUntil { get; set; }
        public int State { get; set; }
        public string? StatePrescription => State switch
        {
            1 => "ACTIVA",
            2 => "DISPENSADA",
            3 => "VENCIDA",
            _ => "CANCELADA"
        };
        public DateTime? AuditCreateDate { get; set; }
    }
}
