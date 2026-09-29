using Microsoft.AspNetCore.Authorization;
using Clinical.UseCases.UseCases.Doctor.Commands.ChangeStateCommand;
using Clinical.UseCases.UseCases.Doctor.Commands.CreateCommand;
using Clinical.UseCases.UseCases.Doctor.Commands.DeleteCommand;
using Clinical.UseCases.UseCases.Doctor.Commands.UpdateCommand;
using Clinical.UseCases.UseCases.Doctor.Queries.GetAllQuery;
using Clinical.UseCases.UseCases.Doctor.Queries.GetByIdQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Clinical.API.Controllers;

[Route("api/[controller]")]
[Authorize]
public class DoctorController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public DoctorController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> ListDoctors()
        => Ok(await _mediator.Send(new GetAllDoctorQuery()));

    [HttpGet("{doctorId:int}")]
    public async Task<IActionResult> GetDoctorById(int doctorId)
        => Ok(await _mediator.Send(new GetDoctorByIdQuery { DoctorId = doctorId }));

    [HttpPost("Register")]
    public async Task<IActionResult> RegisterDoctor([FromBody] CreateDoctorCommand command)
    {
        await _mediator.Send(command);
        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPut("Edit")]
    public async Task<IActionResult> EditDoctor([FromBody] UpdateDoctorCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("Remove/{doctorId:int}")]
    public async Task<IActionResult> RemoveDoctor(int doctorId)
    {
        await _mediator.Send(new DeleteDoctorCommand { DoctorId = doctorId });
        return NoContent();
    }

    [HttpPatch("ChangeState")]
    public async Task<IActionResult> ChangeState([FromBody] ChangeStateDoctorCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
}
