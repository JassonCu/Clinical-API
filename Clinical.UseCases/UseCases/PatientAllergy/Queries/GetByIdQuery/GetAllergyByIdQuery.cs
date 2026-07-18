using Clinical.Application.DTOS.PatientAllergy.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.PatientAllergy.Queries.GetByIdQuery
{
    public class GetAllergyByIdQuery : IRequest<BaseResponse<GetAllAllergyResponseDto>>
    {
        public int AllergyId { get; set; }
    }
}
