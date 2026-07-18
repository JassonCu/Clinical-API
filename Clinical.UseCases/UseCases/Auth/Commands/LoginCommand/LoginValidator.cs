using FluentValidation;

namespace Clinical.UseCases.UseCases.Auth.Commands.LoginCommand;

public class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.Username)
            .NotNull().WithMessage("El usuario no puede ser nulo.")
            .NotEmpty().WithMessage("El usuario no puede ser vacío.");

        RuleFor(x => x.Password)
            .NotNull().WithMessage("La contraseña no puede ser nula.")
            .NotEmpty().WithMessage("La contraseña no puede ser vacía.");
    }
}
