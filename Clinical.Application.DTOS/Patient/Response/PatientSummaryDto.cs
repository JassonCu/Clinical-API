namespace Clinical.Application.DTOS.Patient.Response
{
    /// <summary>Consolidated 360° clinical view of a patient, aggregated for the point of care.</summary>
    public class PatientSummaryDto
    {
        public PatientSummaryHeaderDto Patient { get; set; } = new();
        public IReadOnlyList<SummaryAllergyDto> ActiveAllergies { get; set; } = [];
        public IReadOnlyList<SummaryDiagnosisDto> Diagnoses { get; set; } = [];
        public IReadOnlyList<SummaryPrescriptionDto> RecentPrescriptions { get; set; } = [];
        public IReadOnlyList<SummaryAppointmentDto> UpcomingAppointments { get; set; } = [];
        public SummaryAppointmentDto? LastAppointment { get; set; }
        public SummaryVitalSignDto? LatestVitalSign { get; set; }
        public SummaryMedicalHistoryDto? MedicalHistory { get; set; }
        public PatientSummaryCountsDto Counts { get; set; } = new();
    }

    public class PatientSummaryHeaderDto
    {
        public int PatientId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? DocumentNumber { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public string? BloodType { get; set; }
        public string? Phone { get; set; }
        public string? InsuranceCompany { get; set; }
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContactPhone { get; set; }
    }

    public class SummaryAllergyDto
    {
        public string? AllergenName { get; set; }
        public string? AllergenType { get; set; }
        public string? Severity { get; set; }
        public string? Reaction { get; set; }
    }

    public class SummaryDiagnosisDto
    {
        public string? IcdCode { get; set; }
        public string? Description { get; set; }
        public string? DiagnosisType { get; set; }
        public DateTime? Date { get; set; }
    }

    public class SummaryPrescriptionDto
    {
        public int PrescriptionId { get; set; }
        public DateTime? PrescriptionDate { get; set; }
        public DateTime? ValidUntil { get; set; }
        public string? Notes { get; set; }
        public bool IsActive { get; set; }
    }

    public class SummaryAppointmentDto
    {
        public int AppointmentId { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string? Reason { get; set; }
        public int? State { get; set; }
    }

    public class SummaryVitalSignDto
    {
        public DateTime? MeasuredAt { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
        public decimal? Bmi { get; set; }
        public int? BloodPressureSystolic { get; set; }
        public int? BloodPressureDiastolic { get; set; }
        public int? HeartRate { get; set; }
        public decimal? Temperature { get; set; }
        public int? OxygenSaturation { get; set; }
    }

    public class SummaryMedicalHistoryDto
    {
        public string? ChronicDiseases { get; set; }
        public string? PreviousSurgeries { get; set; }
        public string? CurrentMedications { get; set; }
        public string? FamilyHistory { get; set; }
        public string? Habits { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }

    public class PatientSummaryCountsDto
    {
        public int TotalAppointments { get; set; }
        public int TotalPrescriptions { get; set; }
        public int ActiveAllergies { get; set; }
        public int ActiveDiagnoses { get; set; }
    }
}
