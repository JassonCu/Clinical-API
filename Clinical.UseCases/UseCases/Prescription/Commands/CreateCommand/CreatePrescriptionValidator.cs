using FluentValidation;

namespace Clinical.UseCases.UseCases.Prescription.Commands.CreateCommand
{
    public class CreatePrescriptionValidator : AbstractValidator<CreatePrescriptionCommand>
    {
        public CreatePrescriptionValidator()
        {
            RuleFor(x => x.PatientId)
                .NotNull().WithMessage("El paciente es requerido.")
                .GreaterThan(0).WithMessage("El paciente es inválido.");

            RuleFor(x => x.DoctorId)
                .NotNull().WithMessage("El médico es requerido.")
                .GreaterThan(0).WithMessage("El médico es inválido.");

            RuleFor(x => x.PrescriptionDate)
                .NotNull().WithMessage("La fecha de prescripción es requerida.");

            RuleFor(x => x.ValidUntil)
                .NotNull().WithMessage("La fecha de vigencia es requerida.")
                .GreaterThan(x => x.PrescriptionDate).WithMessage("La vigencia debe ser posterior a la fecha de prescripción.");
        }
    }
}
