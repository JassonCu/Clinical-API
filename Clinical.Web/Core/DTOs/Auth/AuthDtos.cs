using System.ComponentModel.DataAnnotations;

namespace Clinical.Web.Core.DTOs.Auth;

public class LoginRequestDto
{
    [Required(ErrorMessage = "El usuario es requerido")]
    [Display(Name = "Usuario")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es requerida")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; } = string.Empty;
}

public class AuthResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public int UserId { get; set; }
    public bool MustChangePassword { get; set; }
}

public class RefreshTokenRequestDto
{
    public string RefreshToken { get; set; } = string.Empty;
}

public class RegisterRequestDto
{
    [Required] public string Username { get; set; } = string.Empty;
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Required, MinLength(8)] public string Password { get; set; } = string.Empty;
    [Required] public string FirstName { get; set; } = string.Empty;
    [Required] public string LastName { get; set; } = string.Empty;
    [Required] public int RoleId { get; set; }
}

public class SetupInitDto
{
    [Required(ErrorMessage = "El nombre de usuario es requerido")]
    [MinLength(3, ErrorMessage = "Mínimo 3 caracteres")]
    [Display(Name = "Usuario")] public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    [Display(Name = "Email")] public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es requerida")]
    [MinLength(8, ErrorMessage = "Mínimo 8 caracteres")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")] public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirme la contraseña")]
    [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirmar contraseña")] public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es requerido")]
    [Display(Name = "Nombre")] public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es requerido")]
    [Display(Name = "Apellido")] public string LastName { get; set; } = string.Empty;
}

public class ResetPasswordDto
{
    [Required(ErrorMessage = "El token es requerido")]
    [Display(Name = "Token de recuperación")] public string Token { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es requerida")]
    [MinLength(8, ErrorMessage = "Mínimo 8 caracteres")]
    [DataType(DataType.Password)]
    [Display(Name = "Nueva contraseña")] public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirme la contraseña")]
    [Compare(nameof(NewPassword), ErrorMessage = "Las contraseñas no coinciden")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirmar contraseña")] public string ConfirmPassword { get; set; } = string.Empty;
}

public class ChangePasswordDto
{
    [Required(ErrorMessage = "La contraseña es requerida")]
    [MinLength(8, ErrorMessage = "Mínimo 8 caracteres")]
    [DataType(DataType.Password)]
    [Display(Name = "Nueva contraseña")] public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirme la contraseña")]
    [Compare(nameof(NewPassword), ErrorMessage = "Las contraseñas no coinciden")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirmar contraseña")] public string ConfirmPassword { get; set; } = string.Empty;
}

public class GenerateResetTokenResponseDto
{
    public string RawToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public string TargetUsername { get; set; } = string.Empty;
}
