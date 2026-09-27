using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons;
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

        // Resolves the audited record id:
        //  1) an explicit [AuditEntityId] property (use this when a command has several ids), else
        //  2) the single unambiguous int/int? "*Id" property. If there are several candidates we
        //     return null rather than guess the wrong one (a misleading id is worse than none).
        private static string? TryGetEntityId(TRequest request)
        {
            var properties = typeof(TRequest).GetProperties();

            var marked = properties.FirstOrDefault(p => p.IsDefined(typeof(AuditEntityIdAttribute), inherit: true));
            if (marked is not null)
                return Normalize(marked.GetValue(request));

            var candidates = properties
                .Where(p => (p.PropertyType == typeof(int) || p.PropertyType == typeof(int?))
                            && p.Name.EndsWith("Id", StringComparison.Ordinal))
                .ToList();

            return candidates.Count == 1 ? Normalize(candidates[0].GetValue(request)) : null;
        }

        private static string? Normalize(object? value) => value is int id && id > 0 ? id.ToString() : null;
    }
}
