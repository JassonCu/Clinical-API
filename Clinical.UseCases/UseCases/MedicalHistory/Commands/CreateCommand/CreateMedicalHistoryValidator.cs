using FluentValidation;

namespace Clinical.UseCases.UseCases.MedicalHistory.Commands.CreateCommand;

public class CreateMedicalHistoryValidator : AbstractValidator<CreateMedicalHistoryCommand>
{
    public CreateMedicalHistoryValidator()
    {
        RuleFor(x => x.PatientId)
            .GreaterThan(0).WithMessage("El campo PacienteId debe ser mayor a 0.");

        RuleFor(x => x.BloodType)
            .Must(bt => bt is null || new[] { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" }.Contains(bt))
            .WithMessage("El grupo sanguíneo no es válido.");
    }
}
