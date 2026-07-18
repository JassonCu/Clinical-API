using Clinical.UseCases.UseCases.Auth.Commands.ChangePasswordCommand;
using Clinical.UseCases.UseCases.Auth.Commands.LoginCommand;
using Clinical.UseCases.UseCases.Auth.Commands.RefreshTokenCommand;
using Clinical.UseCases.UseCases.Auth.Commands.RegisterCommand;
using Clinical.UseCases.UseCases.Auth.Commands.ResetPasswordCommand;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Clinical.API.Controllers;

[Route("api/[controller]")]
public class AuthController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("Login")]
    [EnableRateLimiting("auth")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
        => PayloadResult(await _mediator.Send(command), StatusCodes.Status401Unauthorized);

    [HttpPost("Register")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        => CommandResult(await _mediator.Send(command), StatusCodes.Status201Created);

    [HttpPost("RefreshToken")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
        => PayloadResult(await _mediator.Send(command), StatusCodes.Status401Unauthorized);

    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        => CommandResult(await _mediator.Send(command), StatusCodes.Status200OK);

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
        => CommandResult(await _mediator.Send(command), StatusCodes.Status200OK);
}
