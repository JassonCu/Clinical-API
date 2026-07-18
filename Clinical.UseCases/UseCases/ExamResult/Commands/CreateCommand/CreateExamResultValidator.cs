using FluentValidation;

namespace Clinical.UseCases.UseCases.ExamResult.Commands.CreateCommand;

public class CreateExamResultValidator : AbstractValidator<CreateExamResultCommand>
{
    public CreateExamResultValidator()
    {
        RuleFor(x => x.PatientId)
            .GreaterThan(0).WithMessage("El campo PacienteId debe ser mayor a 0.");

        RuleFor(x => x.ExamId)
            .GreaterThan(0).WithMessage("El campo ExamenId debe ser mayor a 0.");

        RuleFor(x => x.Result)
            .NotNull().WithMessage("El resultado no puede ser nulo.")
            .NotEmpty().WithMessage("El resultado no puede ser vacío.");

        RuleFor(x => x.ResultDate)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("La fecha del resultado no puede ser futura.");
    }
}
