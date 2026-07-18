using Clinical.Application.DTOS.PatientAllergy.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.PatientAllergy.Queries.GetByPatientQuery
{
    public class GetAllergiesByPatientQuery : IRequest<BaseResponse<IEnumerable<GetAllAllergyResponseDto>>>
    {
        public int PatientId { get; set; }
    }
}
