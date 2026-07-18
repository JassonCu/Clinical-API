using Clinical.UseCases.UseCases.Prescription.Commands.ChangeStateCommand;
using Clinical.UseCases.UseCases.Prescription.Commands.CreateCommand;
using Clinical.UseCases.UseCases.Prescription.Commands.DeleteCommand;
using Clinical.UseCases.UseCases.Prescription.Queries.GetAllQuery;
using Clinical.UseCases.UseCases.Prescription.Queries.GetByDoctorQuery;
using Clinical.UseCases.UseCases.Prescription.Queries.GetByIdQuery;
using Clinical.UseCases.UseCases.Prescription.Queries.GetByPatientQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinical.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PrescriptionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PrescriptionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var response = await _mediator.Send(new GetAllPrescriptionQuery());
            return Ok(response);
        }

        [HttpGet("{prescriptionId:int}")]
        public async Task<IActionResult> GetById(int prescriptionId)
        {
            var response = await _mediator.Send(new GetPrescriptionByIdQuery { PrescriptionId = prescriptionId });
            return Ok(response);
        }

        [HttpGet("ByPatient/{patientId:int}")]
        public async Task<IActionResult> GetByPatient(int patientId)
        {
            var response = await _mediator.Send(new GetPrescriptionsByPatientQuery { PatientId = patientId });
            return Ok(response);
        }

        [HttpGet("ByDoctor/{doctorId:int}")]
        public async Task<IActionResult> GetByDoctor(int doctorId)
        {
            var response = await _mediator.Send(new GetPrescriptionsByDoctorQuery { DoctorId = doctorId });
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] CreatePrescriptionCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("Remove/{prescriptionId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remove(int prescriptionId)
        {
            var response = await _mediator.Send(new DeletePrescriptionCommand { PrescriptionId = prescriptionId });
            return Ok(response);
        }

        [HttpPatch("ChangeState")]
        public async Task<IActionResult> ChangeState([FromBody] ChangeStatePrescriptionCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
