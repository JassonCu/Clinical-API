using Clinical.Application.DTOS.User.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.User.Queries.GetByIdQuery
{
    public class GetUserByIdQuery : IRequest<BaseResponse<UserDetailDto>>
    {
        public int UserId { get; set; }
    }
}
