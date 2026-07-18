using Clinical.Infraestructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Clinical.Infraestructure.Extensions
{
    public static class InyectionExtensions
    {
        public static IServiceCollection AddInyectionInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<JwtTokenService>();
            return services;
        }
    }
}
