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
    public class AppointmentController : ControllerBase
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
            return Ok(response);
        }

        [HttpGet("{appointmentId:int}")]
        public async Task<IActionResult> GetAppointmentById(int appointmentId)
        {
            var response = await _mediator.Send(new GetAppointmentByIdQuery() { AppointmentId = appointmentId });
            return Ok(response);
        }

        [HttpGet("ByPatient/{patientId:int}")]
        public async Task<IActionResult> GetByPatient(int patientId)
        {
            var response = await _mediator.Send(new GetAppointmentsByPatientQuery() { PatientId = patientId });
            return Ok(response);
        }

        [HttpGet("ByDoctor/{doctorId:int}")]
        public async Task<IActionResult> GetByDoctor(int doctorId)
        {
            var response = await _mediator.Send(new GetAppointmentsByDoctorQuery() { DoctorId = doctorId });
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAppointment([FromBody] CreateAppointmentCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> EditAppointment([FromBody] UpdateAppointmentCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("Remove/{appointmentId:int}")]
        public async Task<IActionResult> RemoveAppointment(int appointmentId)
        {
            var response = await _mediator.Send(new DeleteAppointmentCommand() { AppointmentId = appointmentId });
            return Ok(response);
        }

        [HttpPatch("ChangeState")]
        public async Task<IActionResult> ChangeState([FromBody] ChangeStateAppointmentCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
