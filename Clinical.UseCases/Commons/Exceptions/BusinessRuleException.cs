namespace Clinical.UseCases.Commons.Exceptions;

/// <summary>Thrown when a domain/business rule is violated. Mapped to HTTP 422.</summary>
public sealed class BusinessRuleException : AppException
{
    public BusinessRuleException(string message) : base(message) { }
}
