using Clinical.UseCases.Commons.Bases;
using Clinical.UseCases.Commons.Exceptions;
using System.Text.Json;

namespace Clinical.API.Extensions.Middleware
{
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (ValidationExceptions ex)
            {
                context.Response.ContentType = "application/json";
                await JsonSerializer.SerializeAsync(context.Response.Body, new BaseResponse<object>
                {
                    Message = "Errores de validación",
                    Errors = ex.Errors
                });
            }
        }
    }
}
