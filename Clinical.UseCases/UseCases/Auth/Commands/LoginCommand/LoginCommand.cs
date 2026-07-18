using Clinical.Application.DTOS.Auth.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Auth.Commands.LoginCommand;

public class LoginCommand : IRequest<BaseResponse<AuthResponseDto>>
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}
