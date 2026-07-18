namespace Clinical.UseCases.Commons.Exceptions;

/// <summary>
/// Base for expected, business-level failures. These are mapped to specific HTTP
/// status codes by the API exception middleware and are NOT logged as errors.
/// </summary>
public abstract class AppException : Exception
{
    protected AppException(string message) : base(message) { }
}
