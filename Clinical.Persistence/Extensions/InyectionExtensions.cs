﻿using Clinical.Interface.Interfaces;
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
            
            return services;
        }
    }
}
