using Clinical.UseCases.UseCases.VitalSign.Commands.CreateCommand;
using Clinical.UseCases.UseCases.VitalSign.Commands.DeleteCommand;
using Clinical.UseCases.UseCases.VitalSign.Queries.GetAllQuery;
using Clinical.UseCases.UseCases.VitalSign.Queries.GetByPatientQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinical.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VitalSignController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VitalSignController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var response = await _mediator.Send(new GetAllVitalSignQuery());
            return Ok(response);
        }

        [HttpGet("ByPatient/{patientId:int}")]
        public async Task<IActionResult> GetByPatient(int patientId)
        {
            var response = await _mediator.Send(new GetVitalSignsByPatientQuery { PatientId = patientId });
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] CreateVitalSignCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("Remove/{vitalSignId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remove(int vitalSignId)
        {
            var response = await _mediator.Send(new DeleteVitalSignCommand { VitalSignId = vitalSignId });
            return Ok(response);
        }
    }
}
