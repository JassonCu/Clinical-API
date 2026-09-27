using Clinical.Infraestructure.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Clinical.Test.AuthTests
{
    public class LoginAttemptTrackerTests
    {
        private static LoginAttemptTracker Build(int maxAttempts)
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Auth:Lockout:MaxFailedAttempts"] = maxAttempts.ToString(),
                    ["Auth:Lockout:WindowMinutes"] = "15",
                    ["Auth:Lockout:LockoutMinutes"] = "15"
                })
                .Build();
            return new LoginAttemptTracker(new MemoryCache(new MemoryCacheOptions()), config);
        }

        [Fact]
        public void LocksOut_AfterConfiguredFailures()
        {
            var tracker = Build(maxAttempts: 3);
            Assert.False(tracker.IsLockedOut("alice"));

            tracker.RegisterFailure("alice");
            tracker.RegisterFailure("alice");
            Assert.False(tracker.IsLockedOut("alice"));   // 2 < 3

            tracker.RegisterFailure("alice");
            Assert.True(tracker.IsLockedOut("alice"));     // 3rd failure trips the lock
        }

        [Fact]
        public void Reset_ClearsLockAndCounter()
        {
            var tracker = Build(maxAttempts: 2);
            tracker.RegisterFailure("bob");
            tracker.RegisterFailure("bob");
            Assert.True(tracker.IsLockedOut("bob"));

            tracker.Reset("bob");
            Assert.False(tracker.IsLockedOut("bob"));
        }

        [Fact]
        public void Tracking_IsCaseInsensitive_ByUsername()
        {
            var tracker = Build(maxAttempts: 2);
            tracker.RegisterFailure("Alice");
            tracker.RegisterFailure("alice");
            Assert.True(tracker.IsLockedOut("ALICE"));
        }
    }
}
