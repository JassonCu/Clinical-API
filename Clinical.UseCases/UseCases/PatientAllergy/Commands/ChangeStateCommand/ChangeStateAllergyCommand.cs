using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.PatientAllergy.Commands.ChangeStateCommand;

public class ChangeStateAllergyCommand : IRequest<BaseResponse<bool>>
{
    public int? AllergyId { get; set; }
    public int? State { get; set; }
}
