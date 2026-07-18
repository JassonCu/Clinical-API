using Clinical.Interface.Interfaces;
using Clinical.Persistence.Context;
using Clinical.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Clinical.Persistence.Extensions
{
    public static class InyectionExtensions
    {
        public static IServiceCollection AddInyectionPersistence(this IServiceCollection services)
        {
            services.AddSingleton<ApplicationDBContext>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IExamRepository, ExamRepository>();
            services.AddTransient<IAppointmentRepository, AppointmentRepository>();
            services.AddTransient<IExamResultRepository, ExamResultRepository>();
            services.AddTransient<IAuthRepository, AuthRepository>();
            services.AddTransient<IPrescriptionRepository, PrescriptionRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ISetupRepository, SetupRepository>();
            services.AddTransient<IPasswordResetRepository, PasswordResetRepository>();
            return services;
        }
    }
}
