using Clinical.Infraestructure.Services;
using Clinical.Interface.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Clinical.Infraestructure.Extensions
{
    public static class InyectionExtensions
    {
        public static IServiceCollection AddInyectionInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IJwtTokenService, JwtTokenService>();
            services.AddMemoryCache();
            services.AddSingleton<ILoginAttemptTracker, LoginAttemptTracker>();
            services.AddSingleton<IPasswordResetNotifier, NullPasswordResetNotifier>();
            return services;
        }
    }
}
