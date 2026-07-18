using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.PatientAllergy.Commands.DeleteCommand
{
    public class DeleteAllergyCommand : IRequest<BaseResponse<bool>>
    {
        public int AllergyId { get; set; }
    }
}
