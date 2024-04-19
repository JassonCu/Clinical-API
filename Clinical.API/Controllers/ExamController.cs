using Clinical.UseCases.UseCases.Exam.Queries.GetAllQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Clinical.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExamController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("ListExams")]
        public async Task<IActionResult> ListExams()
        {
            var response = await _mediator.Send(new GetAllExamQuery());

            return Ok(response);
        }
    }
}
