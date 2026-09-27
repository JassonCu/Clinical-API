namespace Clinical.Interface.Interfaces
{
    /// <summary>
    /// Records an entry in the clinical audit trail (who did what, to which record, when).
    /// The default implementation writes a structured "Audit" log event; swap it for a
    /// dedicated append-only store if regulation requires tamper-evident persistence.
    /// </summary>
    public interface IAuditLogger
    {
        Task RecordAsync(AuditEntry entry);
    }

    public class AuditEntry
    {
        public string Action { get; set; } = string.Empty;
        public string? EntityId { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? IpAddress { get; set; }
        public string? TraceId { get; set; }
        public string Outcome { get; set; } = "Success";
        public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
    }
}
