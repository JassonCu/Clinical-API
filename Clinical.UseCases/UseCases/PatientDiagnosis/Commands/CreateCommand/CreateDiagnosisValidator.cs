using FluentValidation;

namespace Clinical.UseCases.UseCases.PatientDiagnosis.Commands.CreateCommand
{
    public class CreateDiagnosisValidator : AbstractValidator<CreateDiagnosisCommand>
    {
        public CreateDiagnosisValidator()
        {
            RuleFor(x => x.PatientId)
                .NotNull().WithMessage("El paciente es requerido.")
                .GreaterThan(0).WithMessage("El paciente es inválido.");

            RuleFor(x => x.AppointmentId)
                .NotNull().WithMessage("La cita es requerida.")
                .GreaterThan(0).WithMessage("La cita es inválida.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("La descripción del diagnóstico es requerida.")
                .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres.");

            RuleFor(x => x.IcdCode)
                .MaximumLength(20).WithMessage("El código CIE-10 no puede exceder 20 caracteres.");
        }
    }
}
