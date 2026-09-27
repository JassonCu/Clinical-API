namespace Clinical.UseCases.Commons
{
    /// <summary>
    /// Marks the property of a command that identifies the audited record, so the audit trail
    /// records the correct id even when the command carries several foreign keys.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class AuditEntityIdAttribute : Attribute
    {
    }
}
