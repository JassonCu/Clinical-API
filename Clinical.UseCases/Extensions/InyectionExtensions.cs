using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Behaviours;
using Clinical.UseCases.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Clinical.UseCases.Extensions;

public static class InyectionExtensions
{
    public static IServiceCollection AddInyectionApplication(this IServiceCollection services)
    {
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));
        services.AddValidatorsFromAssemblies(new List<Assembly> { Assembly.GetExecutingAssembly() });
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviours<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuditBehaviour<,>));
        services.AddScoped<IPrescriptionAllergyService, PrescriptionAllergyService>();
        services.AddScoped<IAppointmentConflictChecker, AppointmentConflictChecker>();
        services.AddScoped<IPatientSummaryService, PatientSummaryService>();
        return services;
    }
}