using System.ComponentModel.DataAnnotations;

namespace Clinical.Web.Core.DTOs.VitalSign;

public class VitalSignDto
{
    public int VitalSignId { get; set; }
    public int PatientId { get; set; }
    public string PatientFullName { get; set; } = string.Empty;
    public int? AppointmentId { get; set; }
    public decimal? Weight { get; set; }
    public decimal? Height { get; set; }
    public decimal? Bmi { get; set; }
    public int? BloodPressureSystolic { get; set; }
    public int? BloodPressureDiastolic { get; set; }
    public int? HeartRate { get; set; }
    public decimal? Temperature { get; set; }
    public decimal? OxygenSaturation { get; set; }
    public int? RespiratoryRate { get; set; }
    public decimal? GlucoseLevel { get; set; }
    public string? Notes { get; set; }
    public DateTime MeasuredAt { get; set; }
    public int State { get; set; }
}

public class CreateVitalSignDto
{
    [Required][Display(Name = "Paciente")] public int PatientId { get; set; }
    [Display(Name = "Cita")] public int? AppointmentId { get; set; }
    [Display(Name = "Peso (kg)")] public decimal? Weight { get; set; }
    [Display(Name = "Talla (m)")] public decimal? Height { get; set; }
    [Display(Name = "Presión Sistólica")] public int? BloodPressureSystolic { get; set; }
    [Display(Name = "Presión Diastólica")] public int? BloodPressureDiastolic { get; set; }
    [Display(Name = "Freq. Cardíaca")] public int? HeartRate { get; set; }
    [Display(Name = "Temperatura (°C)")] public decimal? Temperature { get; set; }
    [Display(Name = "Saturación O₂ (%)")] public decimal? OxygenSaturation { get; set; }
    [Display(Name = "Freq. Respiratoria")] public int? RespiratoryRate { get; set; }
    [Display(Name = "Glucemia (mg/dL)")] public decimal? GlucoseLevel { get; set; }
    [Display(Name = "Notas")] public string? Notes { get; set; }
    [Required][Display(Name = "Fecha de medición")] public DateTime MeasuredAt { get; set; } = DateTime.Now;
}
