using FluentValidation;

namespace Clinical.UseCases.UseCases.ExamResult.Commands.UpdateCommand;

public class UpdateExamResultValidator : AbstractValidator<UpdateExamResultCommand>
{
    public UpdateExamResultValidator()
    {
        RuleFor(x => x.ExamResultId)
            .GreaterThan(0).WithMessage("El campo ResultadoId debe ser mayor a 0.");

        RuleFor(x => x.Result)
            .NotEmpty().WithMessage("El resultado no puede ser vacío.")
            .When(x => !string.IsNullOrEmpty(x.Result));

        RuleFor(x => x.ResultDate)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("La fecha del resultado no puede ser futura.")
            .When(x => x.ResultDate.HasValue);
    }
}