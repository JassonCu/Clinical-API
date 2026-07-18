using FluentValidation;

namespace Clinical.UseCases.UseCases.Patient.Commands.CreateCommand;

public class CreatePatientValidator : AbstractValidator<CreatePatientCommand>
{
    public CreatePatientValidator()
    {
        RuleFor(x => x.DocumentNumber)
            .NotNull().WithMessage("El número de documento no puede ser nulo.")
            .NotEmpty().WithMessage("El número de documento no puede ser vacío.")
            .MaximumLength(20).WithMessage("El número de documento no puede superar 20 caracteres.");

        RuleFor(x => x.FirstName)
            .NotNull().WithMessage("El nombre no puede ser nulo.")
            .NotEmpty().WithMessage("El nombre no puede ser vacío.")
            .MaximumLength(100).WithMessage("El nombre no puede superar 100 caracteres.");

        RuleFor(x => x.LastName)
            .NotNull().WithMessage("El apellido no puede ser nulo.")
            .NotEmpty().WithMessage("El apellido no puede ser vacío.")
            .MaximumLength(100).WithMessage("El apellido no puede superar 100 caracteres.");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("El formato del correo electrónico no es válido.")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.Gender)
            .Must(g => g == "M" || g == "F" || g == "O")
            .WithMessage("El género debe ser M (Masculino), F (Femenino) o O (Otro).")
            .When(x => !string.IsNullOrEmpty(x.Gender));

        RuleFor(x => x.BirthDate)
            .LessThan(DateTime.Today).WithMessage("La fecha de nacimiento debe ser anterior a hoy.")
            .When(x => x.BirthDate.HasValue);
    }
}
