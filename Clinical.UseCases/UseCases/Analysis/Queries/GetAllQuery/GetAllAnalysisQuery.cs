using Clinical.Application.DTOS.Analysis.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Analysis.Queries.GetAllQuery
{
    public class GetAllAnalysisQuery : IRequest<BaseResponse<IEnumerable<GetAnalysisResponseDto>>>
    {
    }
}
