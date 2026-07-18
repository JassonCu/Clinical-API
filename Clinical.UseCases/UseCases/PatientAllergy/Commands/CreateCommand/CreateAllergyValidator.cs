using FluentValidation;

namespace Clinical.UseCases.UseCases.PatientAllergy.Commands.CreateCommand
{
    public class CreateAllergyValidator : AbstractValidator<CreateAllergyCommand>
    {
        public CreateAllergyValidator()
        {
            RuleFor(x => x.PatientId)
                .NotNull().WithMessage("El paciente es requerido.")
                .GreaterThan(0).WithMessage("El paciente es inválido.");

            RuleFor(x => x.AllergenName)
                .NotEmpty().WithMessage("El nombre del alérgeno es requerido.")
                .MaximumLength(200).WithMessage("El nombre del alérgeno no puede exceder 200 caracteres.");

            RuleFor(x => x.Severity)
                .Must(s => s is null || new[] { "Leve", "Moderada", "Grave", "Anafilaxia" }.Contains(s))
                .WithMessage("La severidad debe ser: Leve, Moderada, Grave o Anafilaxia.");
        }
    }
}
