using Clinical.Domain.Entities;

namespace Clinical.Interface.Interfaces
{
    public interface IPasswordResetRepository
    {
        Task CreateTokenAsync(int userId, string tokenHash, DateTime expiresAt);
        Task<PasswordResetToken?> GetByHashAsync(string tokenHash);
        Task DeleteAsync(int tokenId);
        Task UpdatePasswordAsync(int userId, string passwordHash);
    }
}
