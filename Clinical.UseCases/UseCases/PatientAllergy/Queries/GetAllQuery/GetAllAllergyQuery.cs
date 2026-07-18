using Clinical.Application.DTOS.PatientAllergy.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.PatientAllergy.Queries.GetAllQuery
{
    public class GetAllAllergyQuery : IRequest<BaseResponse<IEnumerable<GetAllAllergyResponseDto>>>
    {
    }
}
