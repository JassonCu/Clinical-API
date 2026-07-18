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
    public class PatientAllergyController : ApiControllerBase
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
            return DataResult(response);
        }

        [HttpGet("{allergyId:int}")]
        public async Task<IActionResult> GetById(int allergyId)
        {
            var response = await _mediator.Send(new GetAllergyByIdQuery { AllergyId = allergyId });
            return DataResult(response);
        }

        [HttpGet("ByPatient/{patientId:int}")]
        public async Task<IActionResult> GetByPatient(int patientId)
        {
            var response = await _mediator.Send(new GetAllergiesByPatientQuery { PatientId = patientId });
            return DataResult(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] CreateAllergyCommand command)
        {
            var response = await _mediator.Send(command);
            return CommandResult(response, StatusCodes.Status201Created);
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Edit([FromBody] UpdateAllergyCommand command)
        {
            var response = await _mediator.Send(command);
            return CommandResult(response, StatusCodes.Status204NoContent);
        }

        [HttpDelete("Remove/{allergyId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remove(int allergyId)
        {
            var response = await _mediator.Send(new DeleteAllergyCommand { AllergyId = allergyId });
            return CommandResult(response, StatusCodes.Status204NoContent);
        }

        [HttpPatch("ChangeState")]
        public async Task<IActionResult> ChangeState([FromBody] ChangeStateAllergyCommand command)
        {
            var response = await _mediator.Send(command);
            return CommandResult(response, StatusCodes.Status204NoContent);
        }
    }
}
