namespace Clinical.Interface.Interfaces
{
    /// <summary>
    /// Tracks failed login attempts per username and enforces a temporary lockout,
    /// complementing the per-IP rate limiter against brute-force attacks.
    /// </summary>
    public interface ILoginAttemptTracker
    {
        bool IsLockedOut(string username);
        void RegisterFailure(string username);
        void Reset(string username);
    }
}
