using System.Data;
using Clinical.Domain.Entities;
using Clinical.Interface;
using Clinical.Persistence.Context;
using Dapper;

namespace Clinical.Persistence.Repositories
{
    public class AnalysisRepository : IAnalysisRepository
    {
        private readonly ApplicationDBContext _context;

        public AnalysisRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Analysis> AnalysisById(int analysisId)
        {
            using var connection = _context.CreateConnection;

            var query = "uspAnalysisById";

            var parameters = new DynamicParameters();
            parameters.Add("AnalysisId", analysisId);

            var analysis = await connection
                .QuerySingleOrDefaultAsync<Analysis>(query, param: parameters, commandType: CommandType.StoredProcedure);

            return analysis;
        }

        public async Task<IEnumerable<Analysis>> ListAnalysis()
        {
            using var connection = _context.CreateConnection;

            var query = "uspAnalysisList";

            var analysis = await connection.QueryAsync<Analysis>(query, commandType: CommandType.StoredProcedure);

            return analysis;
        }
    }
}
