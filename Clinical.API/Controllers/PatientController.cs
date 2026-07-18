using Microsoft.AspNetCore.Authorization;
using Clinical.UseCases.UseCases.Patient.Commands.ChangeStateCommand;
using Clinical.UseCases.UseCases.Patient.Commands.CreateCommand;
using Clinical.UseCases.UseCases.Patient.Commands.DeleteCommand;
using Clinical.UseCases.UseCases.Patient.Commands.UpdateCommand;
using Clinical.UseCases.UseCases.Patient.Queries.GetAllQuery;
using Clinical.UseCases.UseCases.Patient.Queries.GetByIdQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Clinical.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PatientController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> ListPatients()
        {
            var response = await _mediator.Send(new GetAllPatientQuery());
            return Ok(response);
        }

        [HttpGet("{patientId:int}")]
        public async Task<IActionResult> GetPatientById(int patientId)
        {
            var response = await _mediator.Send(new GetPatientByIdQuery() { PatientId = patientId });
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterPatient([FromBody] CreatePatientCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> EditPatient([FromBody] UpdatePatientCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("Remove/{patientId:int}")]
        public async Task<IActionResult> RemovePatient(int patientId)
        {
            var response = await _mediator.Send(new DeletePatientCommand() { PatientId = patientId });
            return Ok(response);
        }

        [HttpPatch("ChangeState")]
        public async Task<IActionResult> ChangeState([FromBody] ChangeStatePatientCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
