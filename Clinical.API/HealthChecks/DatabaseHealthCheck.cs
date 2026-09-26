using System.Data.Common;
using Clinical.Persistence.Context;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Clinical.API.HealthChecks
{
    /// <summary>
    /// Readiness check that verifies the database is reachable (runs SELECT 1).
    /// Makes /health return 503 when SQL Server is down instead of a misleading 200.
    /// </summary>
    public sealed class DatabaseHealthCheck : IHealthCheck
    {
        private readonly ApplicationDBContext _context;

        public DatabaseHealthCheck(ApplicationDBContext context) => _context = context;

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using var connection = _context.CreateConnection;
                if (connection is DbConnection dbConnection)
                {
                    await dbConnection.OpenAsync(cancellationToken);
                    await using var command = dbConnection.CreateCommand();
                    command.CommandText = "SELECT 1";
                    await command.ExecuteScalarAsync(cancellationToken);
                }
                else
                {
                    connection.Open();
                }

                return HealthCheckResult.Healthy();
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("No se pudo conectar a la base de datos.", ex);
            }
        }
    }
}
