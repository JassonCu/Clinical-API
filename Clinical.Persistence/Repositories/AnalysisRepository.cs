using System.Data;
using Clinical.Domain.Entities;
using Clinical.Interface.Interfaces;
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

        public async Task<bool> AnalysisEdit(Analysis analysis)
        {
            using var connection = _context.CreateConnection;

            var query = "uspAnalysisEdit";

            var parameters = new DynamicParameters();
            parameters.Add("AnalysisId", analysis.AnalysisId);
            parameters.Add("Name", analysis.Name);

            var recordAffected = await connection
                .ExecuteAsync(query, param: parameters, commandType: CommandType.StoredProcedure);

            return recordAffected > 0;
        }

        public async Task<bool> AnalysisRegister(Analysis analysis)
        {
            using var connection = _context.CreateConnection;

            var query = "uspAnalysisRegister";

            var parameters = new DynamicParameters();
            parameters.Add("Name", analysis.Name);
            parameters.Add("State", 1);
            parameters.Add("AuditCreateDate", DateTime.Now);

            var recordAffected = await connection
                .ExecuteAsync(query, param: parameters, commandType: CommandType.StoredProcedure);

            return recordAffected > 0;
        }

        public async Task<bool> AnalysisRemove(int analysisId)
        {
            using var connection = _context.CreateConnection;
            var query = "uspAnalysisRemove";
            
            var parameters = new DynamicParameters();
            parameters.Add("AnalysisId", analysisId);

            var recordAffected = await connection
                .ExecuteAsync(query, param: parameters, commandType: CommandType.StoredProcedure);

            return recordAffected > 0;
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
