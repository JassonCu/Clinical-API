using FluentValidation;

namespace Clinical.UseCases.UseCases.Patient.Commands.UpdateCommand;

public class UpdatePatientValidator : AbstractValidator<UpdatePatientCommand>
{
    public UpdatePatientValidator()
    {
        RuleFor(x => x.PatientId)
            .GreaterThan(0).WithMessage("El campo PacienteId debe ser mayor a 0.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("El nombre no puede ser vacío.")
            .MaximumLength(100).WithMessage("El nombre no puede superar 100 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.FirstName));

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("El apellido no puede ser vacío.")
            .MaximumLength(100).WithMessage("El apellido no puede superar 100 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.LastName));

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("El formato del correo electrónico no es válido.")
            .When(x => !string.IsNullOrEmpty(x.Email));
    }
}
