using Clinical.Application.DTOS.User.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.User.Queries.GetAllQuery
{
    public class GetAllUsersQuery : IRequest<BaseResponse<IEnumerable<UserListDto>>>
    {
    }
}
