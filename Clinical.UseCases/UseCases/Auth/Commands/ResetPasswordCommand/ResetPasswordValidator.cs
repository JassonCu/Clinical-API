using FluentValidation;

namespace Clinical.UseCases.UseCases.Auth.Commands.ResetPasswordCommand
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.Token).NotEmpty().WithMessage("El token es requerido.");
            RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.");
        }
    }
}
