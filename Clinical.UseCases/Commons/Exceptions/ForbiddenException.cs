namespace Clinical.UseCases.Commons.Exceptions;

/// <summary>Thrown when the caller is authenticated but not allowed to perform the action. Mapped to HTTP 403.</summary>
public sealed class ForbiddenException : AppException
{
    public ForbiddenException(string message) : base(message) { }
}
