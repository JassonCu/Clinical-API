using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Clinical.UseCases.Extensions
{
    public static class InyectionExtensions
    {
        public static IServiceCollection AddInyectionApplication(this IServiceCollection services)
        {
            services.AddMediatR(x => x.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
