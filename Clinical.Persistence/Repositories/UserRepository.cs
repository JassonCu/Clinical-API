using System.Data;
using Clinical.Application.DTOS.User.Request;
using Clinical.Application.DTOS.User.Response;
using Clinical.Interface.Interfaces;
using Clinical.Persistence.Context;
using Clinical.Utils.Constants;
using Dapper;

namespace Clinical.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _context;

        public UserRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserListDto>> GetAllUsersAsync()
        {
            using var connection = _context.CreateConnection;
            return await connection.QueryAsync<UserListDto>(
                StoreProcedures.uspUserList,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<UserDetailDto?> GetUserByIdAsync(int userId)
        {
            using var connection = _context.CreateConnection;
            return await connection.QuerySingleOrDefaultAsync<UserDetailDto>(
                StoreProcedures.uspUserById,
                new { UserId = userId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateUserAsync(UpdateUserDto dto)
        {
            using var connection = _context.CreateConnection;
            await connection.ExecuteAsync(
                StoreProcedures.uspUserEdit,
                new { dto.UserId, dto.FirstName, dto.LastName, dto.Email, dto.RoleId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task ChangeUserStateAsync(int userId, int state)
        {
            using var connection = _context.CreateConnection;
            await connection.ExecuteAsync(
                StoreProcedures.uspUserChangeState,
                new { UserId = userId, State = state },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            using var connection = _context.CreateConnection;
            return await connection.QueryAsync<RoleDto>(
                StoreProcedures.uspRoleList,
                commandType: CommandType.StoredProcedure);
        }
    }
}
