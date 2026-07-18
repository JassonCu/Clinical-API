using Clinical.Application.DTOS.VitalSign.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.VitalSign.Queries.GetAllQuery
{
    public class GetAllVitalSignQuery : IRequest<BaseResponse<IEnumerable<GetAllVitalSignResponseDto>>>
    {
    }
}
