namespace Clinical.Application.DTOS.Prescription.Response
{
    public class GetPrescriptionByIdResponseDto
    {
        public int PrescriptionId { get; set; }
        public int PatientId { get; set; }
        public string? PatientFullName { get; set; }
        public int DoctorId { get; set; }
        public string? DoctorFullName { get; set; }
        public int? AppointmentId { get; set; }
        public DateTime? PrescriptionDate { get; set; }
        public DateTime? ValidUntil { get; set; }
        public string? Notes { get; set; }
        public int? State { get; set; }
        public IEnumerable<PrescriptionDetailItemDto>? Details { get; set; }
    }

    public class PrescriptionDetailItemDto
    {
        public int PrescriptionDetailId { get; set; }
        public string? MedicineName { get; set; }
        public string? GenericName { get; set; }
        public string? Concentration { get; set; }
        public int? Quantity { get; set; }
        public string? Dosage { get; set; }
        public string? Frequency { get; set; }
        public string? Duration { get; set; }
        public string? Instructions { get; set; }
    }
}
