using Clinical.Interface.Interfaces;

namespace Clinical.API.Services;

/// <summary>
/// Writes audit entries as structured events under the dedicated "Audit" logging category,
/// so they can be routed to their own sink/file for retention. No sensitive payloads are logged.
/// </summary>
public class SerilogAuditLogger : IAuditLogger
{
    private readonly ILogger _logger;

    public SerilogAuditLogger(ILoggerFactory loggerFactory) => _logger = loggerFactory.CreateLogger("Audit");

    public Task RecordAsync(AuditEntry entry)
    {
        _logger.LogInformation(
            "AUDIT {Action} {Outcome} entity={EntityId} userId={UserId} user={UserName} ip={IpAddress} trace={TraceId} at {TimestampUtc:o}",
            entry.Action, entry.Outcome, entry.EntityId, entry.UserId, entry.UserName,
            entry.IpAddress, entry.TraceId, entry.TimestampUtc);
        return Task.CompletedTask;
    }
}
