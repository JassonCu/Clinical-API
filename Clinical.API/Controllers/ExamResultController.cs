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
    public class ExamResultController : ControllerBase
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
            return Ok(response);
        }

        [HttpGet("{examResultId:int}")]
        public async Task<IActionResult> GetExamResultById(int examResultId)
        {
            var response = await _mediator.Send(new GetExamResultByIdQuery() { ExamResultId = examResultId });
            return Ok(response);
        }

        [HttpGet("ByPatient/{patientId:int}")]
        public async Task<IActionResult> GetByPatient(int patientId)
        {
            var response = await _mediator.Send(new GetExamResultsByPatientQuery() { PatientId = patientId });
            return Ok(response);
        }

        [HttpGet("ByAppointment/{appointmentId:int}")]
        public async Task<IActionResult> GetByAppointment(int appointmentId)
        {
            var response = await _mediator.Send(new GetExamResultsByAppointmentQuery() { AppointmentId = appointmentId });
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterExamResult([FromBody] CreateExamResultCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> EditExamResult([FromBody] UpdateExamResultCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("Remove/{examResultId:int}")]
        public async Task<IActionResult> RemoveExamResult(int examResultId)
        {
            var response = await _mediator.Send(new DeleteExamResultCommand() { ExamResultId = examResultId });
            return Ok(response);
        }

        [HttpPatch("ChangeState")]
        public async Task<IActionResult> ChangeState([FromBody] ChangeStateExamResultCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
