using Clinical.Web.Core.DTOs.Auth;
using Clinical.Web.Core.DTOs.User;

namespace Clinical.Web.Core.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserListDto>> GetAllAsync();
    Task<UserDetailDto?> GetByIdAsync(int id);
    Task<(bool Success, string? Error)> CreateAsync(CreateUserDto dto);
    Task<bool> UpdateAsync(UpdateUserDto dto);
    Task<bool> ChangeStateAsync(ChangeStateUserDto dto);
    Task<IEnumerable<RoleDto>> GetRolesAsync();
    Task<GenerateResetTokenResponseDto?> GenerateResetTokenAsync(int userId);
}
