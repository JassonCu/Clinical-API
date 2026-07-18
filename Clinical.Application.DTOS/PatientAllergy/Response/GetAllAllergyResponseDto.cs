namespace Clinical.Application.DTOS.PatientAllergy.Response
{
    public class GetAllAllergyResponseDto
    {
        public int AllergyId { get; set; }
        public int PatientId { get; set; }
        public string? PatientFullName { get; set; }
        public string? AllergenType { get; set; }
        public string? AllergenName { get; set; }
        public string? Reaction { get; set; }
        public string? Severity { get; set; }
        public string? Notes { get; set; }
        public int? State { get; set; }
        public DateTime? AuditCreateDate { get; set; }
    }
}
