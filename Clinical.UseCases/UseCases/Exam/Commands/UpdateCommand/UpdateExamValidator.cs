using FluentValidation;

namespace Clinical.UseCases.UseCases.Exam.Commands.UpdateCommand;

public class UpdateExamValidator : AbstractValidator<UpdateExamCommand>
{
    public UpdateExamValidator()
    {
        RuleFor(x => x.ExamId)
            .GreaterThan(0).WithMessage("El campo ExamenId debe ser mayor a 0.");

        RuleFor(x => x.Name)
            .NotNull().WithMessage("El campo Nombre no puede ser nulo.")
            .NotEmpty().WithMessage("El campo Nombre no puede ser vacío.")
            .When(x => x.Name != null);

        RuleFor(x => x.AnalysisId)
            .GreaterThan(0).WithMessage("El campo AnalisisId debe ser mayor a 0.");
    }
}