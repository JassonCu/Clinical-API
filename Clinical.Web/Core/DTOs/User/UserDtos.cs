using System.ComponentModel.DataAnnotations;

namespace Clinical.Web.Core.DTOs.User;

public class UserListDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public int State { get; set; }
    public DateTime AuditCreateDate { get; set; }
}

public class UserDetailDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public int State { get; set; }
    public DateTime AuditCreateDate { get; set; }
}

public class CreateUserDto
{
    [Required(ErrorMessage = "El nombre de usuario es requerido")]
    [Display(Name = "Usuario")] public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    [Display(Name = "Email")] public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es requerida")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")] public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es requerido")]
    [Display(Name = "Nombre")] public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es requerido")]
    [Display(Name = "Apellido")] public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El rol es requerido")]
    [Display(Name = "Rol")] public int RoleId { get; set; }
}

public class UpdateUserDto
{
    public int UserId { get; set; }

    [Required(ErrorMessage = "El nombre es requerido")]
    [Display(Name = "Nombre")] public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es requerido")]
    [Display(Name = "Apellido")] public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    [Display(Name = "Email")] public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "El rol es requerido")]
    [Display(Name = "Rol")] public int RoleId { get; set; }
}

public class ChangeStateUserDto
{
    public int UserId { get; set; }
    public int State { get; set; }
}

public class RoleDto
{
    public int RoleId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
