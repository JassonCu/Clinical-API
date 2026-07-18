namespace Clinical.Application.DTOS.MedicalHistory.Response
{
    public class GetMedicalHistoryResponseDto
    {
        public int MedicalHistoryId { get; set; }
        public int PatientId { get; set; }
        public string? PatientFullName { get; set; }
        public string? BloodType { get; set; }
        public string? ChronicDiseases { get; set; }
        public string? PreviousSurgeries { get; set; }
        public string? FamilyHistory { get; set; }
        public string? CurrentMedications { get; set; }
        public string? Habits { get; set; }
        public string? Observations { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public int? State { get; set; }
        public DateTime? AuditCreateDate { get; set; }
    }
}
