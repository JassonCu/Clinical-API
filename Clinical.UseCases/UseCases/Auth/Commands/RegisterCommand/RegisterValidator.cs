using FluentValidation;

namespace Clinical.UseCases.UseCases.Auth.Commands.RegisterCommand;

public class RegisterValidator : AbstractValidator<RegisterCommand>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Username)
            .NotNull().WithMessage("El usuario es requerido.")
            .NotEmpty().WithMessage("El usuario no puede ser vacío.")
            .MinimumLength(4).WithMessage("El usuario debe tener al menos 4 caracteres.")
            .MaximumLength(50).WithMessage("El usuario no puede superar 50 caracteres.");

        RuleFor(x => x.Email)
            .NotNull().WithMessage("El email es requerido.")
            .EmailAddress().WithMessage("El formato del email no es válido.");

        RuleFor(x => x.Password)
            .NotNull().WithMessage("La contraseña es requerida.")
            .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
            .Matches("[A-Z]").WithMessage("La contraseña debe contener al menos una mayúscula.")
            .Matches("[0-9]").WithMessage("La contraseña debe contener al menos un número.")
            .Matches("[^a-zA-Z0-9]").WithMessage("La contraseña debe contener al menos un carácter especial.");

        RuleFor(x => x.RoleId)
            .GreaterThan(0).WithMessage("El rol es requerido.");
    }
}
