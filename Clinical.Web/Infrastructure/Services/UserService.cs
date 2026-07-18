using Clinical.Web.Core.DTOs.Auth;
using Clinical.Web.Core.DTOs.User;
using Clinical.Web.Core.Interfaces;

namespace Clinical.Web.Infrastructure.Services;

public class UserService : BaseApiService, IUserService
{
    public UserService(IHttpClientFactory factory, IHttpContextAccessor accessor, ILogger<UserService> logger)
        : base(factory, accessor, logger) { }

    public Task<IEnumerable<UserListDto>> GetAllAsync() =>
        GetAsync<IEnumerable<UserListDto>>("/api/user").ContinueWith(t => t.Result ?? []);

    public Task<UserDetailDto?> GetByIdAsync(int id) =>
        GetAsync<UserDetailDto>($"/api/user/{id}");

    public Task<IEnumerable<RoleDto>> GetRolesAsync() =>
        GetAsync<IEnumerable<RoleDto>>("/api/user/roles").ContinueWith(t => t.Result ?? []);

    public async Task<(bool Success, string? Error)> CreateAsync(CreateUserDto dto)
    {
        var (ok, error) = await PostAsync("/api/Auth/Register", dto);
        return (ok, error);
    }

    public async Task<bool> UpdateAsync(UpdateUserDto dto)
    {
        var (ok, _) = await PutAsync("/api/user/Edit", dto);
        return ok;
    }

    public async Task<bool> ChangeStateAsync(ChangeStateUserDto dto)
    {
        var (ok, _) = await PatchAsync("/api/user/ChangeState", dto);
        return ok;
    }

    public async Task<GenerateResetTokenResponseDto?> GenerateResetTokenAsync(int userId)
    {
        var (success, data, _) = await PostWithResultAsync<object, GenerateResetTokenResponseDto>(
            $"/api/user/{userId}/reset-password", new { });
        return success ? data : null;
    }
}
