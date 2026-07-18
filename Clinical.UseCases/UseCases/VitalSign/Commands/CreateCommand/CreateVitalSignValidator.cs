using FluentValidation;

namespace Clinical.UseCases.UseCases.VitalSign.Commands.CreateCommand
{
    public class CreateVitalSignValidator : AbstractValidator<CreateVitalSignCommand>
    {
        public CreateVitalSignValidator()
        {
            RuleFor(x => x.PatientId).GreaterThan(0).WithMessage("El PacienteId debe ser mayor a 0.");
            RuleFor(x => x.Weight).GreaterThan(0).WithMessage("El peso debe ser mayor a 0.").When(x => x.Weight.HasValue);
            RuleFor(x => x.Height).GreaterThan(0).WithMessage("La talla debe ser mayor a 0.").When(x => x.Height.HasValue);
            RuleFor(x => x.OxygenSaturation).InclusiveBetween(0, 100).WithMessage("La saturación de oxígeno debe estar entre 0 y 100.").When(x => x.OxygenSaturation.HasValue);
            RuleFor(x => x.MeasuredAt).LessThanOrEqualTo(DateTime.Now).WithMessage("La fecha de medición no puede ser futura.");
        }
    }
}
