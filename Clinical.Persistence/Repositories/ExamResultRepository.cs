using System.Data;
using Clinical.Application.DTOS.ExamResult.Response;
using Clinical.Interface.Interfaces;
using Clinical.Persistence.Context;
using Dapper;

namespace Clinical.Persistence.Repositories
{
    public class ExamResultRepository : IExamResultRepository
    {
        private readonly ApplicationDBContext _context;

        public ExamResultRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GetAllExamResultResponseDto>> GetAllExamResults(string storedProcedure)
        {
            using var connection = _context.CreateConnection;
            return await connection.QueryAsync<GetAllExamResultResponseDto>(storedProcedure, commandType: CommandType.StoredProcedure);
        }

        public async Task<GetExamResultByIdResponseDto> GetExamResultById(string storedProcedure, object parameter)
        {
            using var connection = _context.CreateConnection;
            var objParam = new DynamicParameters(parameter);
            return await connection.QuerySingleOrDefaultAsync<GetExamResultByIdResponseDto>(storedProcedure, param: objParam, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<GetAllExamResultResponseDto>> GetExamResultsByPatient(string storedProcedure, object parameter)
        {
            using var connection = _context.CreateConnection;
            var objParam = new DynamicParameters(parameter);
            return await connection.QueryAsync<GetAllExamResultResponseDto>(storedProcedure, param: objParam, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<GetAllExamResultResponseDto>> GetExamResultsByAppointment(string storedProcedure, object parameter)
        {
            using var connection = _context.CreateConnection;
            var objParam = new DynamicParameters(parameter);
            return await connection.QueryAsync<GetAllExamResultResponseDto>(storedProcedure, param: objParam, commandType: CommandType.StoredProcedure);
        }
    }
}
