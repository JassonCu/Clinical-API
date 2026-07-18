namespace Clinical.UseCases.Commons.Exceptions;

/// <summary>Thrown when an operation conflicts with current state (e.g. duplicates). Mapped to HTTP 409.</summary>
public sealed class ConflictException : AppException
{
    public ConflictException(string message) : base(message) { }
}
