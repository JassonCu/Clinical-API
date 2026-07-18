using Clinical.Application.DTOS.User.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.User.Queries.GetRolesQuery
{
    public class GetAllRolesQuery : IRequest<BaseResponse<IEnumerable<RoleDto>>>
    {
    }
}
