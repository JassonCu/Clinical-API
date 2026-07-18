using Clinical.UseCases.Commons.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Clinical.UseCases.Commons.Behaviours;

/// <summary>
/// Catches unexpected exceptions thrown by handlers, logs them with request context,
/// and rethrows so the API exception middleware can produce a safe response.
/// Expected business exceptions (validation, <see cref="AppException"/>) are rethrown
/// untouched — they are controlled failures mapped to specific status codes upstream.
/// </summary>
public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<UnhandledExceptionBehaviour<TRequest, TResponse>> _logger;

    public UnhandledExceptionBehaviour(ILogger<UnhandledExceptionBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (ValidationExceptions)
        {
            throw;
        }
        catch (AppException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception for request {RequestName}", typeof(TRequest).Name);
            throw;
        }
    }
}
