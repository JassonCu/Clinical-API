using System.ComponentModel.DataAnnotations;

namespace Clinical.Web.Core.DTOs.Prescription;

public class PrescriptionListDto
{
    public int PrescriptionId { get; set; }
    public string PatientFullName { get; set; } = string.Empty;
    public string DoctorFullName { get; set; } = string.Empty;
    public int? AppointmentId { get; set; }
    public DateTime PrescriptionDate { get; set; }
    public DateTime? ValidUntil { get; set; }
    public int State { get; set; }
    public string StatePrescription { get; set; } = string.Empty;
    public DateTime AuditCreateDate { get; set; }
}

public class PrescriptionDetailDto
{
    public int PrescriptionId { get; set; }
    public int PatientId { get; set; }
    public string PatientFullName { get; set; } = string.Empty;
    public int DoctorId { get; set; }
    public string DoctorFullName { get; set; } = string.Empty;
    public int? AppointmentId { get; set; }
    public DateTime PrescriptionDate { get; set; }
    public DateTime? ValidUntil { get; set; }
    public string? Notes { get; set; }
    public int State { get; set; }
    public IEnumerable<PrescriptionDetailItemDto> Details { get; set; } = [];
}

public class PrescriptionDetailItemDto
{
    public int PrescriptionDetailId { get; set; }
    public string MedicineName { get; set; } = string.Empty;
    public string GenericName { get; set; } = string.Empty;
    public string Concentration { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string Dosage { get; set; } = string.Empty;
    public string Frequency { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
    public string? Instructions { get; set; }
}

public class CreatePrescriptionDto
{
    [Required][Display(Name = "Paciente")] public int PatientId { get; set; }
    [Required][Display(Name = "Médico")] public int DoctorId { get; set; }
    [Display(Name = "Cita")] public int? AppointmentId { get; set; }
    [Display(Name = "Fecha")] public DateTime PrescriptionDate { get; set; } = DateTime.Today;
    [Display(Name = "Válida hasta")] public DateTime? ValidUntil { get; set; }
    [Display(Name = "Notas")] public string? Notes { get; set; }
    public List<CreatePrescriptionDetailDto> Details { get; set; } = [];
}

public class CreatePrescriptionDetailDto
{
    [Required] public int MedicineId { get; set; }
    [Required][Range(1, int.MaxValue)] public int Quantity { get; set; }
    [Required] public string Dosage { get; set; } = string.Empty;
    [Required] public string Frequency { get; set; } = string.Empty;
    [Required] public string Duration { get; set; } = string.Empty;
    public string? Instructions { get; set; }
}

public class ChangeStatePrescriptionDto
{
    public int PrescriptionId { get; set; }
    public int State { get; set; }
}
