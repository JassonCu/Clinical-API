using Clinical.UseCases.UseCases.User.Commands.ChangeStateCommand;
using Clinical.UseCases.UseCases.User.Commands.GenerateResetTokenCommand;
using Clinical.UseCases.UseCases.User.Commands.UpdateCommand;
using Clinical.UseCases.UseCases.User.Queries.GetAllQuery;
using Clinical.UseCases.UseCases.User.Queries.GetByIdQuery;
using Clinical.UseCases.UseCases.User.Queries.GetRolesQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinical.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UserController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> ListUsers()
            => DataResult(await _mediator.Send(new GetAllUsersQuery()));

        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetUserById(int userId)
            => DataResult(await _mediator.Send(new GetUserByIdQuery { UserId = userId }));

        [HttpPut("Edit")]
        public async Task<IActionResult> EditUser([FromBody] UpdateUserCommand command)
            => CommandResult(await _mediator.Send(command), StatusCodes.Status204NoContent);

        [HttpPatch("ChangeState")]
        public async Task<IActionResult> ChangeState([FromBody] ChangeStateUserCommand command)
            => CommandResult(await _mediator.Send(command), StatusCodes.Status204NoContent);

        [HttpGet("roles")]
        public async Task<IActionResult> ListRoles()
            => DataResult(await _mediator.Send(new GetAllRolesQuery()));

        [HttpPost("{userId:int}/reset-password")]
        public async Task<IActionResult> GenerateResetToken(int userId)
            => PayloadResult(await _mediator.Send(new GenerateResetTokenCommand { UserId = userId }));
    }
}
