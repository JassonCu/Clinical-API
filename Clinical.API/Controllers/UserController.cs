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
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> ListUsers()
        {
            var response = await _mediator.Send(new GetAllUsersQuery());
            return Ok(response);
        }

        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            var response = await _mediator.Send(new GetUserByIdQuery { UserId = userId });
            return Ok(response);
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> EditUser([FromBody] UpdateUserCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPatch("ChangeState")]
        public async Task<IActionResult> ChangeState([FromBody] ChangeStateUserCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpGet("roles")]
        public async Task<IActionResult> ListRoles()
        {
            var response = await _mediator.Send(new GetAllRolesQuery());
            return Ok(response);
        }

        [HttpPost("{userId:int}/reset-password")]
        public async Task<IActionResult> GenerateResetToken(int userId)
        {
            var response = await _mediator.Send(new GenerateResetTokenCommand { UserId = userId });
            return Ok(response);
        }
    }
}
