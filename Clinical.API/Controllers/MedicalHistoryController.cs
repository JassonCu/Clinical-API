using Clinical.UseCases.UseCases.MedicalHistory.Commands.CreateCommand;
using Clinical.UseCases.UseCases.MedicalHistory.Commands.DeleteCommand;
using Clinical.UseCases.UseCases.MedicalHistory.Commands.UpdateCommand;
using Clinical.UseCases.UseCases.MedicalHistory.Queries.GetByIdQuery;
using Clinical.UseCases.UseCases.MedicalHistory.Queries.GetByPatientQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinical.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MedicalHistoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MedicalHistoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{medicalHistoryId:int}")]
        public async Task<IActionResult> GetById(int medicalHistoryId)
        {
            var response = await _mediator.Send(new GetMedicalHistoryByIdQuery { MedicalHistoryId = medicalHistoryId });
            return Ok(response);
        }

        [HttpGet("ByPatient/{patientId:int}")]
        public async Task<IActionResult> GetByPatient(int patientId)
        {
            var response = await _mediator.Send(new GetMedicalHistoryByPatientQuery { PatientId = patientId });
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] CreateMedicalHistoryCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Edit([FromBody] UpdateMedicalHistoryCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("Remove/{medicalHistoryId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remove(int medicalHistoryId)
        {
            var response = await _mediator.Send(new DeleteMedicalHistoryCommand { MedicalHistoryId = medicalHistoryId });
            return Ok(response);
        }
    }
}
