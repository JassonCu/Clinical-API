using Microsoft.AspNetCore.Authorization;
using Clinical.UseCases.UseCases.ExamResult.Commands.ChangeStateCommand;
using Clinical.UseCases.UseCases.ExamResult.Commands.CreateCommand;
using Clinical.UseCases.UseCases.ExamResult.Commands.DeleteCommand;
using Clinical.UseCases.UseCases.ExamResult.Commands.UpdateCommand;
using Clinical.UseCases.UseCases.ExamResult.Queries.GetAllQuery;
using Clinical.UseCases.UseCases.ExamResult.Queries.GetByAppointmentQuery;
using Clinical.UseCases.UseCases.ExamResult.Queries.GetByIdQuery;
using Clinical.UseCases.UseCases.ExamResult.Queries.GetByPatientQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Clinical.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExamResultController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public ExamResultController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> ListExamResults()
        {
            var response = await _mediator.Send(new GetAllExamResultQuery());
            return DataResult(response);
        }

        [HttpGet("{examResultId:int}")]
        public async Task<IActionResult> GetExamResultById(int examResultId)
        {
            var response = await _mediator.Send(new GetExamResultByIdQuery() { ExamResultId = examResultId });
            return DataResult(response);
        }

        [HttpGet("ByPatient/{patientId:int}")]
        public async Task<IActionResult> GetByPatient(int patientId)
        {
            var response = await _mediator.Send(new GetExamResultsByPatientQuery() { PatientId = patientId });
            return DataResult(response);
        }

        [HttpGet("ByAppointment/{appointmentId:int}")]
        public async Task<IActionResult> GetByAppointment(int appointmentId)
        {
            var response = await _mediator.Send(new GetExamResultsByAppointmentQuery() { AppointmentId = appointmentId });
            return DataResult(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterExamResult([FromBody] CreateExamResultCommand command)
        {
            var response = await _mediator.Send(command);
            return CommandResult(response, StatusCodes.Status201Created);
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> EditExamResult([FromBody] UpdateExamResultCommand command)
        {
            var response = await _mediator.Send(command);
            return CommandResult(response, StatusCodes.Status204NoContent);
        }

        [HttpDelete("Remove/{examResultId:int}")]
        public async Task<IActionResult> RemoveExamResult(int examResultId)
        {
            var response = await _mediator.Send(new DeleteExamResultCommand() { ExamResultId = examResultId });
            return CommandResult(response, StatusCodes.Status204NoContent);
        }

        [HttpPatch("ChangeState")]
        public async Task<IActionResult> ChangeState([FromBody] ChangeStateExamResultCommand command)
        {
            var response = await _mediator.Send(command);
            return CommandResult(response, StatusCodes.Status204NoContent);
        }
    }
}
