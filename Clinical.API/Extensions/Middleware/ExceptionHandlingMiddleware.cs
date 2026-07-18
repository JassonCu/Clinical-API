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
            catch (ValidationExceptions ex)
            {
                await WriteValidationAsync(context, ex);
            }
            catch (NotFoundException ex)
            {
                await WriteAsync(context, HttpStatusCode.NotFound, "Recurso no encontrado", ex.Message);
            }
            catch (ConflictException ex)
            {
                await WriteAsync(context, HttpStatusCode.Conflict, "Conflicto", ex.Message);
            }
            catch (ForbiddenException ex)
            {
                await WriteAsync(context, HttpStatusCode.Forbidden, "Acceso denegado", ex.Message);
            }
            catch (BusinessRuleException ex)
            {
                await WriteAsync(context, HttpStatusCode.UnprocessableEntity, "Regla de negocio no cumplida", ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt.");
                await WriteAsync(context, HttpStatusCode.Unauthorized, "No autorizado", "No autorizado.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception.");
                await WriteAsync(context, HttpStatusCode.InternalServerError, "Error interno",
                    "Ocurrió un error inesperado. Contacte al administrador.");
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
