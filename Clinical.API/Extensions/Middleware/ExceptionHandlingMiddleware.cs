using Clinical.UseCases.Commons.Bases;
using Clinical.UseCases.Commons.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Clinical.API.Extensions.Middleware
{
    /// <summary>
    /// Single place that translates exceptions into RFC 9457 problem responses
    /// (application/problem+json). Expected business exceptions map to specific status
    /// codes; anything unexpected becomes a 500 with a generic detail (logged, never leaked).
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                // If the response already started, we can't write a clean error body without
                // corrupting it — log and rethrow so the original failure isn't masked.
                if (context.Response.HasStarted)
                {
                    _logger.LogError(ex, "The response has already started; cannot write an error response.");
                    throw;
                }

                switch (ex)
                {
                    case ValidationExceptions validation:
                        await WriteValidationAsync(context, validation);
                        break;
                    case NotFoundException:
                        await WriteAsync(context, HttpStatusCode.NotFound, "Recurso no encontrado", ex.Message);
                        break;
                    case ConflictException:
                        await WriteAsync(context, HttpStatusCode.Conflict, "Conflicto", ex.Message);
                        break;
                    case ForbiddenException:
                        await WriteAsync(context, HttpStatusCode.Forbidden, "Acceso denegado", ex.Message);
                        break;
                    case BusinessRuleException:
                        await WriteAsync(context, HttpStatusCode.UnprocessableEntity, "Regla de negocio no cumplida", ex.Message);
                        break;
                    case UnauthorizedAccessException:
                        _logger.LogWarning(ex, "Unauthorized access attempt.");
                        await WriteAsync(context, HttpStatusCode.Unauthorized, "No autorizado", "No autorizado.");
                        break;
                    default:
                        _logger.LogError(ex, "Unhandled exception.");
                        await WriteAsync(context, HttpStatusCode.InternalServerError, "Error interno",
                            "Ocurrió un error inesperado. Contacte al administrador.");
                        break;
                }
            }
        }

        private static Task WriteAsync(HttpContext context, HttpStatusCode status, string title, string detail)
        {
            var problem = new ProblemDetails
            {
                Status = (int)status,
                Title = title,
                Detail = detail,
                Type = $"https://httpstatuses.io/{(int)status}",
                Instance = context.Request.Path
            };
            return WriteProblemAsync(context, problem);
        }

        private static Task WriteValidationAsync(HttpContext context, ValidationExceptions ex)
        {
            var errors = (ex.Errors ?? Enumerable.Empty<BaseError>())
                .GroupBy(e => e.PropertyName ?? string.Empty)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage ?? string.Empty).ToArray());

            var problem = new ValidationProblemDetails(errors)
            {
                Status = (int)HttpStatusCode.BadRequest,
                Title = "Errores de validación",
                Type = "https://httpstatuses.io/400",
                Instance = context.Request.Path
            };
            return WriteProblemAsync(context, problem);
        }

        private static Task WriteProblemAsync(HttpContext context, ProblemDetails problem)
        {
            problem.Extensions["traceId"] = context.TraceIdentifier;
            context.Response.StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/problem+json";
            return context.Response.WriteAsync(JsonSerializer.Serialize(problem, problem.GetType(), JsonOptions));
        }
    }
}
