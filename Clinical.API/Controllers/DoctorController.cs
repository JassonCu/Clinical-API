using Microsoft.AspNetCore.Authorization;
using Clinical.UseCases.UseCases.Doctor.Commands.ChangeStateCommand;
using Clinical.UseCases.UseCases.Doctor.Commands.CreateCommand;
using Clinical.UseCases.UseCases.Doctor.Commands.DeleteCommand;
using Clinical.UseCases.UseCases.Doctor.Commands.UpdateCommand;
using Clinical.UseCases.UseCases.Doctor.Queries.GetAllQuery;
using Clinical.UseCases.UseCases.Doctor.Queries.GetByIdQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Clinical.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DoctorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DoctorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> ListDoctors()
        {
            var response = await _mediator.Send(new GetAllDoctorQuery());
            return Ok(response);
        }

        [HttpGet("{doctorId:int}")]
        public async Task<IActionResult> GetDoctorById(int doctorId)
        {
            var response = await _mediator.Send(new GetDoctorByIdQuery() { DoctorId = doctorId });
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterDoctor([FromBody] CreateDoctorCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> EditDoctor([FromBody] UpdateDoctorCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("Remove/{doctorId:int}")]
        public async Task<IActionResult> RemoveDoctor(int doctorId)
        {
            var response = await _mediator.Send(new DeleteDoctorCommand() { DoctorId = doctorId });
            return Ok(response);
        }

        [HttpPatch("ChangeState")]
        public async Task<IActionResult> ChangeState([FromBody] ChangeStateDoctorCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
