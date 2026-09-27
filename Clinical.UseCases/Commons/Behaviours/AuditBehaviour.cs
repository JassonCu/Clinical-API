using Clinical.Interface.Interfaces;
using MediatR;

namespace Clinical.UseCases.Commons.Behaviours
{
    /// <summary>
    /// Writes a clinical audit entry for every state-changing operation (commands): who performed
    /// which action, on which record, when, and whether it succeeded. Read queries are not audited.
    /// </summary>
    public class AuditBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IAuditLogger _audit;
        private readonly ICurrentUserService _currentUser;

        public AuditBehaviour(IAuditLogger audit, ICurrentUserService currentUser)
        {
            _audit = audit;
            _currentUser = currentUser;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var action = typeof(TRequest).Name;

            // Audit mutations only; reads (queries) are not recorded.
            if (!action.EndsWith("Command", StringComparison.Ordinal))
                return await next();

            try
            {
                var response = await next();
                await RecordAsync(action, request, "Success");
                return response;
            }
            catch
            {
                await RecordAsync(action, request, "Failed");
                throw;
            }
        }

        private Task RecordAsync(string action, TRequest request, string outcome) =>
            _audit.RecordAsync(new AuditEntry
            {
                Action = action,
                EntityId = TryGetEntityId(request),
                UserId = _currentUser.UserId,
                UserName = _currentUser.UserName,
                IpAddress = _currentUser.IpAddress,
                TraceId = _currentUser.TraceId,
                Outcome = outcome,
                TimestampUtc = DateTime.UtcNow
            });

        // Best-effort: the target record id from an int "*Id" property on the command, if any.
        private static string? TryGetEntityId(TRequest request)
        {
            var property = typeof(TRequest).GetProperties()
                .FirstOrDefault(p => p.PropertyType == typeof(int) && p.Name.EndsWith("Id", StringComparison.Ordinal));

            return property?.GetValue(request) is int id && id > 0 ? id.ToString() : null;
        }
    }
}
