using Microsoft.AspNetCore.Authorization;
using Clinical.UseCases.UseCases.Analysis.Commands.ChangeStateCommand;
using Clinical.UseCases.UseCases.Analysis.Commands.CreateCommand;
using Clinical.UseCases.UseCases.Analysis.Commands.DeleteCommand;
using Clinical.UseCases.UseCases.Analysis.Commands.UpdateCommand;
using Clinical.UseCases.UseCases.Analysis.Queries.GetAllQuery;
using Clinical.UseCases.UseCases.Analysis.Queries.GetByIdQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Clinical.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AnalysisController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public AnalysisController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> ListAnalysis()
        {
            var response = await _mediator.Send(new GetAllAnalysisQuery());

            return DataResult(response);
        }

        [HttpGet("{analysisId:int}")]
        public async Task<IActionResult> GetAnalysisById(int analysisId)
        {
            var response = await _mediator.Send(new GetAnalysisByIdQuery() { AnalysisId = analysisId });

            return DataResult(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAnalysis([FromBody] CreateAnalysisCommand command)
        {
            var response = await _mediator.Send(command);

            return CommandResult(response, StatusCodes.Status201Created);
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> EditAnalysis([FromBody] UpdateAnalysisCommand command)
        {
            var response = await _mediator.Send(command);
            
            return CommandResult(response, StatusCodes.Status204NoContent);
        }

        [HttpDelete ("Remove/{analysisId:int}")]
        public async Task<IActionResult> RemoveAnalysis(int analysisId)
        {
            var response = await _mediator.Send(new DeleteAnalysisCommand() { AnalysisId = analysisId });

            return CommandResult(response, StatusCodes.Status204NoContent);
        }

        [HttpPatch ("ChangeState")]
        public async Task<IActionResult> ChangeState([FromBody] ChangeStateAnalysisCommand command)
        {
            var response = await _mediator.Send(command);

            return CommandResult(response, StatusCodes.Status204NoContent);
        }
    }
}