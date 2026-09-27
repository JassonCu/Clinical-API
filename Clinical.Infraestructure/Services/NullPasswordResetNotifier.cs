using Clinical.Interface.Interfaces;

namespace Clinical.Infraestructure.Services
{
    /// <summary>
    /// No delivery channel configured: reports "not delivered" so the reset token is returned
    /// to the admin caller for manual, secure sharing. Replace with an email-based notifier
    /// in production to stop exposing the raw token in the API response.
    /// </summary>
    public class NullPasswordResetNotifier : IPasswordResetNotifier
    {
        public Task<bool> TryNotifyAsync(string username, string? email, string rawToken, DateTime expiresAt, CancellationToken cancellationToken)
            => Task.FromResult(false);
    }
}
