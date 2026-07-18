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
