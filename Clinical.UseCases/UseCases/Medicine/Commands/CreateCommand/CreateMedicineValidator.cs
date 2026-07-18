using FluentValidation;

namespace Clinical.UseCases.UseCases.Medicine.Commands.CreateCommand;

public class CreateMedicineValidator : AbstractValidator<CreateMedicineCommand>
{
    public CreateMedicineValidator()
    {
        RuleFor(x => x.Code).NotNull().NotEmpty().WithMessage("El código del medicamento es requerido.");
        RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("El nombre del medicamento es requerido.").MaximumLength(200);
        RuleFor(x => x.GenericName).NotNull().NotEmpty().WithMessage("El nombre genérico es requerido.");
        RuleFor(x => x.CurrentStock).GreaterThanOrEqualTo(0).WithMessage("El stock no puede ser negativo.");
        RuleFor(x => x.MinimumStock).GreaterThanOrEqualTo(0).WithMessage("El stock mínimo no puede ser negativo.");
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("El precio no puede ser negativo.");
    }
}
