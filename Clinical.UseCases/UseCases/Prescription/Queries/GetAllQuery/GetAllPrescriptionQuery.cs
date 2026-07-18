using Clinical.Application.DTOS.Prescription.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Prescription.Queries.GetAllQuery
{
    public class GetAllPrescriptionQuery : IRequest<BaseResponse<IEnumerable<GetAllPrescriptionResponseDto>>>
    {
    }
}
