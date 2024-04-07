using FluentValidation;

namespace Clinical.UseCases.UseCases.Analysis.Commands.CreateCommand
{
    public class CreateAnalysisValidator : AbstractValidator<CreateAnalysisCommand>
    {
        public CreateAnalysisValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("El campo Nombre no puede ser nulo.")
                .When(x => x.Name != null) // Aplicar la validación solo si el valor no es nulo
                .NotEmpty().WithMessage("El campo Nombre no puede ser vacío");
        }
    }
}
