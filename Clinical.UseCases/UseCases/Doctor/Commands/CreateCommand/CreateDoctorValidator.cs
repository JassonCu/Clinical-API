using FluentValidation;

namespace Clinical.UseCases.UseCases.Doctor.Commands.CreateCommand;

public class CreateDoctorValidator : AbstractValidator<CreateDoctorCommand>
{
    public CreateDoctorValidator()
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

        RuleFor(x => x.Specialty)
            .NotNull().WithMessage("La especialidad no puede ser nula.")
            .NotEmpty().WithMessage("La especialidad no puede ser vacía.")
            .MaximumLength(100).WithMessage("La especialidad no puede superar 100 caracteres.");

        RuleFor(x => x.MedicalLicense)
            .NotNull().WithMessage("El número de colegiatura no puede ser nulo.")
            .NotEmpty().WithMessage("El número de colegiatura no puede ser vacío.");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("El formato del correo electrónico no es válido.")
            .When(x => !string.IsNullOrEmpty(x.Email));
    }
}
