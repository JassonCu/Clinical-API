using System.ComponentModel.DataAnnotations;

namespace Clinical.Web.Core.DTOs.Doctor;

public class DoctorListDto
{
    public int DoctorId { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public int State { get; set; }
    public string StateDoctor { get; set; } = string.Empty;
    public DateTime AuditCreateDate { get; set; }
}

public class DoctorDetailDto
{
    public int DoctorId { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string MedicalLicense { get; set; } = string.Empty;
    public int State { get; set; }
    public DateTime AuditCreateDate { get; set; }
}

public class CreateDoctorDto
{
    [Required(ErrorMessage = "El número de documento es requerido")]
    [Display(Name = "N° Documento")] public string DocumentNumber { get; set; } = string.Empty;
    [Required(ErrorMessage = "El nombre es requerido")]
    [Display(Name = "Nombre")] public string FirstName { get; set; } = string.Empty;
    [Required(ErrorMessage = "El apellido es requerido")]
    [Display(Name = "Apellido")] public string LastName { get; set; } = string.Empty;
    [Required(ErrorMessage = "La especialidad es requerida")]
    [Display(Name = "Especialidad")] public string Specialty { get; set; } = string.Empty;
    [Display(Name = "Sub-especialidad")] public string? Subspecialty { get; set; }
    [EmailAddress][Display(Name = "Email")] public string? Email { get; set; }
    [Display(Name = "Teléfono")] public string? Phone { get; set; }
    [Display(Name = "Celular")] public string? MobilePhone { get; set; }
    [Display(Name = "Tarjeta Profesional")] public string? MedicalLicense { get; set; }
    [Display(Name = "Tarifa Consulta")] public decimal? ConsultationFee { get; set; }
    [Display(Name = "Horario")] public string? WorkingSchedule { get; set; }
    [Display(Name = "Biografía")] public string? Biography { get; set; }
}

public class UpdateDoctorDto : CreateDoctorDto
{
    public int DoctorId { get; set; }
}

public class ChangeStateDoctorDto
{
    public int DoctorId { get; set; }
    public int State { get; set; }
}
