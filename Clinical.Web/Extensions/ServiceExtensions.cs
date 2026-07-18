using Clinical.Web.Core.Interfaces;
using Clinical.Web.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace Clinical.Web.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddHttpContextAccessor();

        services.AddHttpClient("ClinicalAPI", client =>
        {
            client.BaseAddress = new Uri(config["ApiSettings:BaseUrl"]
                ?? throw new InvalidOperationException("ApiSettings:BaseUrl is not configured"));
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ISetupService, SetupService>();
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IMedicineService, MedicineService>();
        services.AddScoped<IPrescriptionService, PrescriptionService>();
        services.AddScoped<IVitalSignService, VitalSignService>();
        services.AddScoped<IMedicalHistoryService, MedicalHistoryService>();
        services.AddScoped<IPatientAllergyService, PatientAllergyService>();
        services.AddScoped<IPatientDiagnosisService, PatientDiagnosisService>();
        services.AddScoped<IExamService, ExamService>();
        services.AddScoped<IAnalysisService, AnalysisService>();
        services.AddScoped<IExamResultService, ExamResultService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }

    public static IServiceCollection AddCookieAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Auth/Login";
                options.LogoutPath = "/Auth/Logout";
                options.AccessDeniedPath = "/Auth/AccessDenied";
                options.Cookie.Name = "ClinicalWeb.Auth";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.ExpireTimeSpan = TimeSpan.FromHours(8);
                options.SlidingExpiration = true;
                options.Events.OnRedirectToLogin = ctx =>
                {
                    if (ctx.Request.Path.StartsWithSegments("/api"))
                    {
                        ctx.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    }
                    ctx.Response.Redirect(ctx.RedirectUri);
                    return Task.CompletedTask;
                };
            });

        return services;
    }

    public static IServiceCollection AddWebRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("login", opt =>
            {
                opt.PermitLimit = 5;
                opt.Window = TimeSpan.FromMinutes(1);
                opt.QueueLimit = 0;
                opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });
            options.AddFixedWindowLimiter("global", opt =>
            {
                opt.PermitLimit = 200;
                opt.Window = TimeSpan.FromMinutes(1);
                opt.QueueLimit = 0;
            });
            options.RejectionStatusCode = 429;
        });

        return services;
    }
}
