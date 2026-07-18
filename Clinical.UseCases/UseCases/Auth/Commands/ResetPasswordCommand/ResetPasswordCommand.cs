using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Auth.Commands.ResetPasswordCommand
{
    public class ResetPasswordCommand : IRequest<BaseResponse<bool>>
    {
        public string? Token { get; set; }
        public string? NewPassword { get; set; }
    }
}
