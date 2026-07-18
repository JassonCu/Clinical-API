using System.ComponentModel.DataAnnotations;

namespace Clinical.Web.Core.DTOs.Appointment;

public class AppointmentListDto
{
    public int AppointmentId { get; set; }
    public string PatientFullName { get; set; } = string.Empty;
    public string DoctorFullName { get; set; } = string.Empty;
    public string DoctorSpecialty { get; set; } = string.Empty;
    public DateTime AppointmentDate { get; set; }
    public string Reason { get; set; } = string.Empty;
    public int State { get; set; }
    public string StateAppointment { get; set; } = string.Empty;
    public DateTime AuditCreateDate { get; set; }
}

public class AppointmentDetailDto
{
    public int AppointmentId { get; set; }
    public int PatientId { get; set; }
    public string PatientFullName { get; set; } = string.Empty;
    public int DoctorId { get; set; }
    public string DoctorFullName { get; set; } = string.Empty;
    public string DoctorSpecialty { get; set; } = string.Empty;
    public DateTime AppointmentDate { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? Diagnosis { get; set; }
    public string? Notes { get; set; }
    public int State { get; set; }
    public DateTime AuditCreateDate { get; set; }
}

public class CreateAppointmentDto
{
    [Required(ErrorMessage = "El paciente es requerido")]
    [Display(Name = "Paciente")] public int PatientId { get; set; }
    [Required(ErrorMessage = "El médico es requerido")]
    [Display(Name = "Médico")] public int DoctorId { get; set; }
    [Required(ErrorMessage = "La fecha es requerida")]
    [Display(Name = "Fecha y Hora")] public DateTime AppointmentDate { get; set; } = DateTime.Now;
    [Required(ErrorMessage = "El motivo es requerido")]
    [Display(Name = "Motivo")] public string Reason { get; set; } = string.Empty;
    [Display(Name = "Diagnóstico")] public string? Diagnosis { get; set; }
    [Display(Name = "Notas")] public string? Notes { get; set; }
}

public class UpdateAppointmentDto : CreateAppointmentDto
{
    public int AppointmentId { get; set; }
}

public class ChangeStateAppointmentDto
{
    public int AppointmentId { get; set; }
    public int State { get; set; }
}
