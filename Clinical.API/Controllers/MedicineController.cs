using Clinical.UseCases.UseCases.Medicine.Commands.ChangeStateCommand;
using Clinical.UseCases.UseCases.Medicine.Commands.CreateCommand;
using Clinical.UseCases.UseCases.Medicine.Commands.DeleteCommand;
using Clinical.UseCases.UseCases.Medicine.Commands.UpdateCommand;
using Clinical.UseCases.UseCases.Medicine.Queries.GetAllQuery;
using Clinical.UseCases.UseCases.Medicine.Queries.GetByIdQuery;
using Clinical.UseCases.UseCases.Medicine.Queries.GetLowStockQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinical.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MedicineController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MedicineController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var response = await _mediator.Send(new GetAllMedicineQuery());
            return Ok(response);
        }

        [HttpGet("{medicineId:int}")]
        public async Task<IActionResult> GetById(int medicineId)
        {
            var response = await _mediator.Send(new GetMedicineByIdQuery { MedicineId = medicineId });
            return Ok(response);
        }

        [HttpGet("LowStock")]
        public async Task<IActionResult> GetLowStock()
        {
            var response = await _mediator.Send(new GetLowStockMedicineQuery());
            return Ok(response);
        }

        [HttpPost("Register")]
        [Authorize(Roles = "Admin,Pharmacist")]
        public async Task<IActionResult> Register([FromBody] CreateMedicineCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut("Edit")]
        [Authorize(Roles = "Admin,Pharmacist")]
        public async Task<IActionResult> Edit([FromBody] UpdateMedicineCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("Remove/{medicineId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remove(int medicineId)
        {
            var response = await _mediator.Send(new DeleteMedicineCommand { MedicineId = medicineId });
            return Ok(response);
        }

        [HttpPatch("ChangeState")]
        [Authorize(Roles = "Admin,Pharmacist")]
        public async Task<IActionResult> ChangeState([FromBody] ChangeStateMedicineCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
