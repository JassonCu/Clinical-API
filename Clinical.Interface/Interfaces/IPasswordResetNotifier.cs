namespace Clinical.Interface.Interfaces
{
    /// <summary>
    /// Delivers a password-reset token to the user through a secure channel (e.g. email).
    /// Returns <c>true</c> when delivered directly, so the API does not need to return the raw
    /// token to the caller. The default implementation performs no delivery (returns false),
    /// which keeps the admin-mediated flow working until a real channel is wired.
    /// </summary>
    public interface IPasswordResetNotifier
    {
        Task<bool> TryNotifyAsync(string username, string? email, string rawToken, DateTime expiresAt, CancellationToken cancellationToken);
    }
}
