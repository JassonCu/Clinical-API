using Clinical.Interface.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Clinical.Infraestructure.Services
{
    /// <summary>
    /// In-memory login-attempt tracker. After <c>Auth:Lockout:MaxFailedAttempts</c> failures
    /// within the window, the username is locked for <c>Auth:Lockout:LockoutMinutes</c>.
    /// Keyed by the submitted username (existent or not) so it never reveals whether an account exists.
    /// Note: state is per-instance; for a multi-instance deployment back it with a distributed cache.
    /// </summary>
    public class LoginAttemptTracker : ILoginAttemptTracker
    {
        private readonly IMemoryCache _cache;
        private readonly int _maxAttempts;
        private readonly TimeSpan _window;
        private readonly TimeSpan _lockout;
        // Serializes the read-modify-write below so concurrent failures can't undercount the
        // attempts (which would let a parallel brute-force exceed the limit). Login volume is low,
        // so contention is negligible.
        private readonly object _sync = new();

        public LoginAttemptTracker(IMemoryCache cache, IConfiguration configuration)
        {
            _cache = cache;
            var section = configuration.GetSection("Auth:Lockout");
            _maxAttempts = int.TryParse(section["MaxFailedAttempts"], out var m) && m > 0 ? m : 5;
            _window = TimeSpan.FromMinutes(int.TryParse(section["WindowMinutes"], out var w) && w > 0 ? w : 15);
            _lockout = TimeSpan.FromMinutes(int.TryParse(section["LockoutMinutes"], out var l) && l > 0 ? l : 15);
        }

        private static string FailKey(string username) => $"login-fail:{username.ToLowerInvariant()}";
        private static string LockKey(string username) => $"login-lock:{username.ToLowerInvariant()}";

        public bool IsLockedOut(string username) => _cache.TryGetValue(LockKey(username), out _);

        public void RegisterFailure(string username)
        {
            lock (_sync)
            {
                var key = FailKey(username);
                var count = _cache.TryGetValue<int>(key, out var current) ? current + 1 : 1;

                if (count >= _maxAttempts)
                {
                    _cache.Set(LockKey(username), true, _lockout);
                    _cache.Remove(key);
                }
                else
                {
                    _cache.Set(key, count, _window);
                }
            }
        }

        public void Reset(string username)
        {
            _cache.Remove(FailKey(username));
            _cache.Remove(LockKey(username));
        }
    }
}
