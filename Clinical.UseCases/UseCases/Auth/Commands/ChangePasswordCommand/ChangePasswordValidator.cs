using FluentValidation;

namespace Clinical.UseCases.UseCases.Auth.Commands.ChangePasswordCommand
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("Usuario inválido.");
            RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.");
        }
    }
}
