using Clinical.Web.Core.DTOs.Auth;
using Clinical.Web.Core.Interfaces;

namespace Clinical.Web.Infrastructure.Services;

public class SetupService : BaseApiService, ISetupService
{
    public SetupService(IHttpClientFactory factory, IHttpContextAccessor accessor, ILogger<SetupService> logger)
        : base(factory, accessor, logger) { }

    public async Task<bool> IsInitializedAsync()
    {
        var result = await GetAsync<bool>("/api/setup/status");
        return result;
    }

    public async Task<(bool Success, string? Error)> InitAsync(SetupInitDto dto)
    {
        var (ok, error) = await PostAsync("/api/setup/init", new
        {
            dto.Username,
            dto.Email,
            dto.Password,
            dto.FirstName,
            dto.LastName
        });
        return (ok, error);
    }
}
