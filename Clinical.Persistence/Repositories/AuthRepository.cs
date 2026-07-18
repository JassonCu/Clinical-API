using System.Data;
using Clinical.Domain.Entities;
using Clinical.Interface.Interfaces;
using Clinical.Persistence.Context;
using Dapper;

namespace Clinical.Persistence.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDBContext _context;

        public AuthRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            using var connection = _context.CreateConnection;
            return await connection.QuerySingleOrDefaultAsync<User>(
                "uspUserByUsername",
                new { Username = username },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            using var connection = _context.CreateConnection;
            return await connection.QuerySingleOrDefaultAsync<User>(
                "uspUserByEmail",
                new { Email = email },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> RegisterUserAsync(User user)
        {
            using var connection = _context.CreateConnection;
            var rows = await connection.ExecuteAsync(
                "uspUserRegister",
                new { user.Username, user.Email, user.PasswordHash, user.FirstName, user.LastName, user.RoleId, MustChangePassword = user.MustChangePassword ?? true },
                commandType: CommandType.StoredProcedure);
            return rows > 0;
        }

        public async Task<bool> UpdateRefreshTokenAsync(int userId, string refreshToken, DateTime expiry)
        {
            using var connection = _context.CreateConnection;
            var rows = await connection.ExecuteAsync(
                "uspUserUpdateRefreshToken",
                new { UserId = userId, RefreshToken = refreshToken, RefreshTokenExpiry = expiry },
                commandType: CommandType.StoredProcedure);
            return rows > 0;
        }

        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            using var connection = _context.CreateConnection;
            return await connection.QuerySingleOrDefaultAsync<User>(
                "uspUserByRefreshToken",
                new { RefreshToken = refreshToken },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<string?> GetRoleNameAsync(int roleId)
        {
            using var connection = _context.CreateConnection;
            return await connection.QuerySingleOrDefaultAsync<string>(
                "uspRoleById",
                new { RoleId = roleId },
                commandType: CommandType.StoredProcedure);
        }
    }
}
