using System.Data;
using Clinical.Domain.Entities;
using Clinical.Interface.Interfaces;
using Clinical.Persistence.Context;
using Dapper;

namespace Clinical.Persistence.Repositories
{
    public class PasswordResetRepository : IPasswordResetRepository
    {
        private readonly ApplicationDBContext _context;
        public PasswordResetRepository(ApplicationDBContext context) => _context = context;

        public async Task CreateTokenAsync(int userId, string tokenHash, DateTime expiresAt)
        {
            using var connection = _context.CreateConnection;
            await connection.ExecuteAsync(
                "uspPasswordResetTokenCreate",
                new { UserId = userId, TokenHash = tokenHash, ExpiresAt = expiresAt },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<PasswordResetToken?> GetByHashAsync(string tokenHash)
        {
            using var connection = _context.CreateConnection;
            return await connection.QuerySingleOrDefaultAsync<PasswordResetToken>(
                "uspPasswordResetTokenByHash",
                new { TokenHash = tokenHash },
                commandType: CommandType.StoredProcedure);
        }

        public async Task DeleteAsync(int tokenId)
        {
            using var connection = _context.CreateConnection;
            await connection.ExecuteAsync(
                "uspPasswordResetTokenDelete",
                new { TokenId = tokenId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdatePasswordAsync(int userId, string passwordHash)
        {
            using var connection = _context.CreateConnection;
            await connection.ExecuteAsync(
                "uspUserUpdatePassword",
                new { UserId = userId, PasswordHash = passwordHash },
                commandType: CommandType.StoredProcedure);
        }
    }
}
