using Clinical.UseCases.Commons.Bases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Clinical.API.Controllers;

/// <summary>
/// Base for API controllers. Two response conventions coexist during the incremental
/// "raw type" migration (see Stage 2):
/// <list type="bullet">
///   <item><b>Legacy</b> (most controllers): handlers return <see cref="BaseResponse{T}"/>; use the
///   <c>DataResult</c>/<c>CommandResult</c>/<c>PayloadResult</c> helpers, which unwrap it to raw
///   data + status code (the envelope never crosses the wire).</item>
///   <item><b>Target</b> (piloted by <c>DoctorController</c>): handlers return the raw type / <c>Unit</c>
///   and throw typed exceptions on failure; the controller returns <c>Ok(...)</c>/201/204 directly,
///   without these helpers.</item>
/// </list>
/// New features should follow the target (Doctor) pattern; the helpers below exist for the
/// not-yet-migrated controllers.
/// </summary>
[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    /// <summary>Authenticated user id (JWT sub), or 0 if unauthenticated.</summary>
    protected int CurrentUserId =>
        int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub"), out var id) ? id : 0;

    /// <summary>Authenticated username (JWT unique_name), or empty if unauthenticated.</summary>
    protected string CurrentUsername =>
        User.FindFirstValue(ClaimTypes.Name) ?? User.FindFirstValue("unique_name") ?? string.Empty;

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
        {
            // A command failed WITHOUT throwing a typed exception (NotFound/Conflict/BusinessRule).
            // That path degrades to a generic 400; log it so the omission is visible, not silent.
            HttpContext.RequestServices.GetService<ILogger<ApiControllerBase>>()?
                .LogWarning("Command returned an untyped failure (degraded to 400): {Message}", response.Message);
            return Problem(detail: response.Message ?? "Operación fallida.", statusCode: StatusCodes.Status400BadRequest);
        }

        return successStatus switch
        {
            StatusCodes.Status201Created => StatusCode(StatusCodes.Status201Created),
            StatusCodes.Status204NoContent => NoContent(),
            _ => Ok()
        };
    }
}
