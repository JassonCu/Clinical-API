using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Clinical.UseCases.Extensions
{
    public static class InyectionExtensions
    {
        public static IServiceCollection AddInyectionApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
