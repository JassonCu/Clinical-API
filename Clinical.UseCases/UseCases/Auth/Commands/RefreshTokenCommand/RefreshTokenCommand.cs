using Clinical.Application.DTOS.Auth.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Auth.Commands.RefreshTokenCommand;

public class RefreshTokenCommand : IRequest<BaseResponse<AuthResponseDto>>
{
    public string? RefreshToken { get; set; }
}