using Clinical.UseCases.UseCases.Setup.Commands.SetupInitCommand;
using Clinical.UseCases.UseCases.Setup.Queries.GetSetupStatusQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinical.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class SetupController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SetupController(IMediator mediator) => _mediator = mediator;

        [HttpGet("status")]
        public async Task<IActionResult> Status()
        {
            var response = await _mediator.Send(new GetSetupStatusQuery());
            return Ok(response);
        }

        [HttpPost("init")]
        public async Task<IActionResult> Init([FromBody] SetupInitCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
