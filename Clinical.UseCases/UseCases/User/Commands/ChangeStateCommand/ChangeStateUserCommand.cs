using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.User.Commands.ChangeStateCommand
{
    public class ChangeStateUserCommand : IRequest<BaseResponse<bool>>
    {
        public int UserId { get; set; }
        public int State { get; set; }
    }
}
