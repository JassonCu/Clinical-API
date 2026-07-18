using Clinical.UseCases.Commons.Bases;
using Microsoft.AspNetCore.Mvc;

namespace Clinical.API.Controllers
{
    /// <summary>
    /// Base for API controllers. Translates the internal handler result (<see cref="BaseResponse{T}"/>)
    /// into a pure REST response: raw data + proper status code on success, ProblemDetails on failure.
    /// The envelope never crosses the wire.
    /// </summary>
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        /// <summary>Query result → 200 OK with the payload (or 404, thrown upstream when not found).</summary>
        protected IActionResult DataResult<T>(BaseResponse<T> response) => Ok(response.Data);

        /// <summary>Auth/payload result → 200 OK with data on success; ProblemDetails otherwise.</summary>
        protected IActionResult PayloadResult<T>(BaseResponse<T> response, int failureStatus = StatusCodes.Status400BadRequest)
            => response.IsSuccess
                ? Ok(response.Data)
                : Problem(detail: response.Message, statusCode: failureStatus);

        /// <summary>Command result → given success status (201/204/200) on success; ProblemDetails otherwise.</summary>
        protected IActionResult CommandResult(BaseResponse<bool> response, int successStatus)
        {
            if (!response.IsSuccess || !response.Data)
                return Problem(detail: response.Message ?? "Operación fallida.", statusCode: StatusCodes.Status400BadRequest);

            return successStatus switch
            {
                StatusCodes.Status201Created => StatusCode(StatusCodes.Status201Created),
                StatusCodes.Status204NoContent => NoContent(),
                _ => Ok()
            };
        }
    }
}
