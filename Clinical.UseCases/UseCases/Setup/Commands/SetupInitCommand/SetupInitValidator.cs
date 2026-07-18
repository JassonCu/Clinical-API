using FluentValidation;

namespace Clinical.UseCases.UseCases.Setup.Commands.SetupInitCommand
{
    public class SetupInitValidator : AbstractValidator<SetupInitCommand>
    {
        public SetupInitValidator()
        {
            RuleFor(x => x.Username).NotEmpty().MinimumLength(3).WithMessage("El usuario debe tener al menos 3 caracteres.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email inválido.");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.");
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("El nombre es requerido.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("El apellido es requerido.");
        }
    }
}
