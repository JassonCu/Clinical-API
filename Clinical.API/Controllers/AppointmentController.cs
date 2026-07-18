using Microsoft.AspNetCore.Authorization;
using Clinical.UseCases.UseCases.Appointment.Commands.ChangeStateCommand;
using Clinical.UseCases.UseCases.Appointment.Commands.CreateCommand;
using Clinical.UseCases.UseCases.Appointment.Commands.DeleteCommand;
using Clinical.UseCases.UseCases.Appointment.Commands.UpdateCommand;
using Clinical.UseCases.UseCases.Appointment.Queries.GetAllQuery;
using Clinical.UseCases.UseCases.Appointment.Queries.GetByDoctorQuery;
using Clinical.UseCases.UseCases.Appointment.Queries.GetByIdQuery;
using Clinical.UseCases.UseCases.Appointment.Queries.GetByPatientQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Clinical.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppointmentController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public AppointmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> ListAppointments()
        {
            var response = await _mediator.Send(new GetAllAppointmentQuery());
            return DataResult(response);
        }

        [HttpGet("{appointmentId:int}")]
        public async Task<IActionResult> GetAppointmentById(int appointmentId)
        {
            var response = await _mediator.Send(new GetAppointmentByIdQuery() { AppointmentId = appointmentId });
            return DataResult(response);
        }

        [HttpGet("ByPatient/{patientId:int}")]
        public async Task<IActionResult> GetByPatient(int patientId)
        {
            var response = await _mediator.Send(new GetAppointmentsByPatientQuery() { PatientId = patientId });
            return DataResult(response);
        }

        [HttpGet("ByDoctor/{doctorId:int}")]
        public async Task<IActionResult> GetByDoctor(int doctorId)
        {
            var response = await _mediator.Send(new GetAppointmentsByDoctorQuery() { DoctorId = doctorId });
            return DataResult(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAppointment([FromBody] CreateAppointmentCommand command)
        {
            var response = await _mediator.Send(command);
            return CommandResult(response, StatusCodes.Status201Created);
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> EditAppointment([FromBody] UpdateAppointmentCommand command)
        {
            var response = await _mediator.Send(command);
            return CommandResult(response, StatusCodes.Status204NoContent);
        }

        [HttpDelete("Remove/{appointmentId:int}")]
        public async Task<IActionResult> RemoveAppointment(int appointmentId)
        {
            var response = await _mediator.Send(new DeleteAppointmentCommand() { AppointmentId = appointmentId });
            return CommandResult(response, StatusCodes.Status204NoContent);
        }

        [HttpPatch("ChangeState")]
        public async Task<IActionResult> ChangeState([FromBody] ChangeStateAppointmentCommand command)
        {
            var response = await _mediator.Send(command);
            return CommandResult(response, StatusCodes.Status204NoContent);
        }
    }
}
