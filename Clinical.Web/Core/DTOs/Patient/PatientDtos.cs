using System.ComponentModel.DataAnnotations;

namespace Clinical.Web.Core.DTOs.Patient;

public class PatientListDto
{
    public int PatientId { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public int State { get; set; }
    public string StatePatient { get; set; } = string.Empty;
    public DateTime AuditCreateDate { get; set; }
}

public class PatientDetailDto
{
    public int PatientId { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int State { get; set; }
    public DateTime AuditCreateDate { get; set; }
}

public class CreatePatientDto
{
    [Required(ErrorMessage = "El número de documento es requerido")]
    [Display(Name = "N° Documento")] public string DocumentNumber { get; set; } = string.Empty;
    [Display(Name = "Tipo Documento")] public string DocumentType { get; set; } = "CC";
    [Required(ErrorMessage = "El nombre es requerido")]
    [Display(Name = "Nombre")] public string FirstName { get; set; } = string.Empty;
    [Required(ErrorMessage = "El apellido es requerido")]
    [Display(Name = "Apellido")] public string LastName { get; set; } = string.Empty;
    [EmailAddress(ErrorMessage = "Email inválido")]
    [Display(Name = "Email")] public string? Email { get; set; }
    [Display(Name = "Teléfono")] public string? Phone { get; set; }
    [Display(Name = "Celular")] public string? MobilePhone { get; set; }
    [Display(Name = "Fecha de Nacimiento")] public DateTime? BirthDate { get; set; }
    [Display(Name = "Género")] public string? Gender { get; set; }
    [Display(Name = "Dirección")] public string? Address { get; set; }
    [Display(Name = "Ciudad")] public string? City { get; set; }
    [Display(Name = "Grupo Sanguíneo")] public string? BloodType { get; set; }
    [Display(Name = "EPS")] public string? InsuranceCompany { get; set; }
    [Display(Name = "N° Póliza")] public string? InsuranceNumber { get; set; }
    [Display(Name = "Contacto Emergencia")] public string? EmergencyContactName { get; set; }
    [Display(Name = "Tel. Emergencia")] public string? EmergencyContactPhone { get; set; }
}

public class UpdatePatientDto : CreatePatientDto
{
    public int PatientId { get; set; }
}

public class ChangeStatePatientDto
{
    public int PatientId { get; set; }
    public int State { get; set; }
}

// ---- Consolidated 360° clinical summary (read model) ----

public class PatientSummaryDto
{
    public PatientSummaryHeaderDto Patient { get; set; } = new();
    public List<SummaryAllergyDto> ActiveAllergies { get; set; } = [];
    public List<SummaryDiagnosisDto> Diagnoses { get; set; } = [];
    public List<SummaryPrescriptionDto> RecentPrescriptions { get; set; } = [];
    public List<SummaryAppointmentDto> UpcomingAppointments { get; set; } = [];
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
