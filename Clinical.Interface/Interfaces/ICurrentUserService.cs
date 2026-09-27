namespace Clinical.Interface.Interfaces
{
    /// <summary>
    /// Exposes the identity of the caller for the current request (from the authenticated token).
    /// Implemented in the API layer over IHttpContextAccessor, keeping ASP.NET out of the domain.
    /// </summary>
    public interface ICurrentUserService
    {
        int? UserId { get; }
        string? UserName { get; }
        string? IpAddress { get; }
        string? TraceId { get; }
    }
}
