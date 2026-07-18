using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Auth.Commands.ChangePasswordCommand
{
    public class ChangePasswordCommand : IRequest<BaseResponse<bool>>
    {
        public int UserId { get; set; }
        public string? NewPassword { get; set; }
    }
}
