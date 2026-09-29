namespace Clinical.Application.DTOS.Prescription.Response
{
    /// <summary>Result of checking a set of medicines against a patient's registered allergies.</summary>
    public class AllergyCheckResponseDto
    {
        public bool HasConflicts { get; set; }
        public IEnumerable<AllergyConflictDto> Conflicts { get; set; } = [];
    }

    public class AllergyConflictDto
    {
        public int MedicineId { get; set; }
        public string MedicineName { get; set; } = string.Empty;
        public string AllergenName { get; set; } = string.Empty;
        public string? Severity { get; set; }
        public string? Reaction { get; set; }
    }
}
