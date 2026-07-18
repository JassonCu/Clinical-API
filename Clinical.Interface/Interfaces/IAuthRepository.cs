using Clinical.Domain.Entities;

namespace Clinical.Interface.Interfaces
{
    public interface IAuthRepository
    {
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> RegisterUserAsync(User user);
        Task<bool> UpdateRefreshTokenAsync(int userId, string refreshToken, DateTime expiry);
        Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
        Task<string?> GetRoleNameAsync(int roleId);
    }
}
