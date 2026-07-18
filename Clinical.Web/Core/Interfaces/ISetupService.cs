using Clinical.Web.Core.DTOs.Auth;

namespace Clinical.Web.Core.Interfaces;

public interface ISetupService
{
    Task<bool> IsInitializedAsync();
    Task<(bool Success, string? Error)> InitAsync(SetupInitDto dto);
}
