using Clinical.UseCases.UseCases.PatientAllergy.Commands.ChangeStateCommand;
using Clinical.UseCases.UseCases.PatientAllergy.Commands.CreateCommand;
using Clinical.UseCases.UseCases.PatientAllergy.Commands.DeleteCommand;
using Clinical.UseCases.UseCases.PatientAllergy.Commands.UpdateCommand;
using Clinical.UseCases.UseCases.PatientAllergy.Queries.GetAllQuery;
using Clinical.UseCases.UseCases.PatientAllergy.Queries.GetByIdQuery;
using Clinical.UseCases.UseCases.PatientAllergy.Queries.GetByPatientQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinical.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientAllergyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PatientAllergyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var response = await _mediator.Send(new GetAllAllergyQuery());
            return Ok(response);
        }

        [HttpGet("{allergyId:int}")]
        public async Task<IActionResult> GetById(int allergyId)
        {
            var response = await _mediator.Send(new GetAllergyByIdQuery { AllergyId = allergyId });
            return Ok(response);
        }

        [HttpGet("ByPatient/{patientId:int}")]
        public async Task<IActionResult> GetByPatient(int patientId)
        {
            var response = await _mediator.Send(new GetAllergiesByPatientQuery { PatientId = patientId });
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] CreateAllergyCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Edit([FromBody] UpdateAllergyCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("Remove/{allergyId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remove(int allergyId)
        {
            var response = await _mediator.Send(new DeleteAllergyCommand { AllergyId = allergyId });
            return Ok(response);
        }

        [HttpPatch("ChangeState")]
        public async Task<IActionResult> ChangeState([FromBody] ChangeStateAllergyCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
