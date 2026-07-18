using Clinical.Application.DTOS.User.Request;
using Clinical.Application.DTOS.User.Response;

namespace Clinical.Interface.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserListDto>> GetAllUsersAsync();
        Task<UserDetailDto?> GetUserByIdAsync(int userId);
        Task UpdateUserAsync(UpdateUserDto dto);
        Task ChangeUserStateAsync(int userId, int state);
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
    }
}
