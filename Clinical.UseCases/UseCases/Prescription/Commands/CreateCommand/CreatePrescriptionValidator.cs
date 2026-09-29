using FluentValidation;

namespace Clinical.UseCases.UseCases.Prescription.Commands.CreateCommand
{
    public class CreatePrescriptionValidator : AbstractValidator<CreatePrescriptionCommand>
    {
        public CreatePrescriptionValidator()
        {
            RuleFor(x => x.PatientId).NotNull().GreaterThan(0).WithMessage("El paciente es requerido.");
            RuleFor(x => x.DoctorId).NotNull().GreaterThan(0).WithMessage("El médico es requerido.");
            RuleFor(x => x.Details).NotEmpty().WithMessage("La receta debe incluir al menos un medicamento.");

            RuleForEach(x => x.Details).ChildRules(detail =>
            {
                detail.RuleFor(d => d.MedicineId).GreaterThan(0).WithMessage("Medicamento inválido.");
                detail.RuleFor(d => d.Quantity).GreaterThan(0).WithMessage("La cantidad debe ser mayor a 0.");
            });
        }
    }
}
