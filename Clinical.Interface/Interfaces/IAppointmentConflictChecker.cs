using Clinical.Domain.Entities;

namespace Clinical.Interface.Interfaces;

/// <summary>
/// Detects scheduling conflicts (double-booking) for a doctor within a configurable time slot.
/// Shared by appointment creation and rescheduling.
/// </summary>
public interface IAppointmentConflictChecker
{
    /// <summary>Returns the conflicting appointment for the doctor near <paramref name="when"/>, or null.</summary>
    Task<Appointment?> FindConflictAsync(int doctorId, DateTime when, int? excludeAppointmentId = null);
}
