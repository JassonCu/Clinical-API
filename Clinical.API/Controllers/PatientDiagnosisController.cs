using Clinical.UseCases.UseCases.PatientDiagnosis.Commands.ChangeStateCommand;
using Clinical.UseCases.UseCases.PatientDiagnosis.Commands.CreateCommand;
using Clinical.UseCases.UseCases.PatientDiagnosis.Commands.DeleteCommand;
using Clinical.UseCases.UseCases.PatientDiagnosis.Queries.GetAllQuery;
using Clinical.UseCases.UseCases.PatientDiagnosis.Queries.GetByAppointmentQuery;
using Clinical.UseCases.UseCases.PatientDiagnosis.Queries.GetByIdQuery;
using Clinical.UseCases.UseCases.PatientDiagnosis.Queries.GetByPatientQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinical.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientDiagnosisController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public PatientDiagnosisController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var response = await _mediator.Send(new GetAllDiagnosisQuery());
            return DataResult(response);
        }

        [HttpGet("{diagnosisId:int}")]
        public async Task<IActionResult> GetById(int diagnosisId)
        {
            var response = await _mediator.Send(new GetDiagnosisByIdQuery { DiagnosisId = diagnosisId });
            return DataResult(response);
        }

        [HttpGet("ByPatient/{patientId:int}")]
        public async Task<IActionResult> GetByPatient(int patientId)
        {
            var response = await _mediator.Send(new GetDiagnosesByPatientQuery { PatientId = patientId });
            return DataResult(response);
        }

        [HttpGet("ByAppointment/{appointmentId:int}")]
        public async Task<IActionResult> GetByAppointment(int appointmentId)
        {
            var response = await _mediator.Send(new GetDiagnosesByAppointmentQuery { AppointmentId = appointmentId });
            return DataResult(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] CreateDiagnosisCommand command)
        {
            var response = await _mediator.Send(command);
            return CommandResult(response, StatusCodes.Status201Created);
        }

        [HttpDelete("Remove/{diagnosisId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remove(int diagnosisId)
        {
            var response = await _mediator.Send(new DeleteDiagnosisCommand { DiagnosisId = diagnosisId });
            return CommandResult(response, StatusCodes.Status204NoContent);
        }

        [HttpPatch("ChangeState")]
        public async Task<IActionResult> ChangeState([FromBody] ChangeStateDiagnosisCommand command)
        {
            var response = await _mediator.Send(command);
            return CommandResult(response, StatusCodes.Status204NoContent);
        }
    }
}
