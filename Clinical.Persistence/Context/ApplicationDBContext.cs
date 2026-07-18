using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Clinical.Persistence.Context
{
    public class ApplicationDBContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public ApplicationDBContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("ClinicalConnection")!;
        }

        public IDbConnection CreateConnection => new SqlConnection(_connectionString);
    }
}
