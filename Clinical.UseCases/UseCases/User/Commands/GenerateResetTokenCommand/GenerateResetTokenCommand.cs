using Clinical.Application.DTOS.Auth.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.User.Commands.GenerateResetTokenCommand
{
    public class GenerateResetTokenCommand : IRequest<BaseResponse<GenerateResetTokenResponseDto>>
    {
        public int UserId { get; set; }
    }
}
