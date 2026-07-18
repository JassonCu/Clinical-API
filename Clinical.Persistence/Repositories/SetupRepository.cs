using System.Data;
using Clinical.Interface.Interfaces;
using Clinical.Persistence.Context;
using Dapper;

namespace Clinical.Persistence.Repositories
{
    public class SetupRepository : ISetupRepository
    {
        private readonly ApplicationDBContext _context;
        public SetupRepository(ApplicationDBContext context) => _context = context;

        public async Task<bool> IsInitializedAsync()
        {
            using var connection = _context.CreateConnection;
            return await connection.QuerySingleAsync<bool>(
                "uspSetupIsInitialized",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> InitAsync(string username, string email, string passwordHash, string firstName, string lastName)
        {
            using var connection = _context.CreateConnection;
            await connection.ExecuteAsync(
                "uspSetupInit",
                new { Username = username, Email = email, PasswordHash = passwordHash, FirstName = firstName, LastName = lastName },
                commandType: CommandType.StoredProcedure);
            return true; // RAISERROR throws on failure; reaching here means success
        }
    }
}
