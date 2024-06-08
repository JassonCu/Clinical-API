using System.Data;
using Clinical.Application.DTOS.Exam.Response;
using Clinical.Interface.Interfaces;
using Clinical.Persistence.Context;
using Dapper;

namespace Clinical.Persistence.Repositories
{
    public class ExamRepository : IExamRepository
    {
        private readonly ApplicationDBContext _context;

        public ExamRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GetAllExamResponseDto>> GetAllExam(string storedProcedure)
        {
            using var connection = _context.CreateConnection;
            var exams = await connection.QueryAsync<GetAllExamResponseDto>(storedProcedure, commandType: CommandType.StoredProcedure);

            return exams;
        }
    }
}
