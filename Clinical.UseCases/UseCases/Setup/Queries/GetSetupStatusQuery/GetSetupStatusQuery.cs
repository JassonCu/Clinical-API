using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Setup.Queries.GetSetupStatusQuery
{
    public class GetSetupStatusQuery : IRequest<BaseResponse<bool>> { }
}
