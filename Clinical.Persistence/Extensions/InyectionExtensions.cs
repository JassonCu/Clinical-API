using Clinical.Persistence.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Clinical.Persistence.Extensions
{
    public static class InyectionExtensions
    {
        public static IServiceCollection AddInyectionPersistence(this IServiceCollection services)
        {
            services.AddSingleton<ApplicationDBContext>();

            return services;
        }
    }
}
