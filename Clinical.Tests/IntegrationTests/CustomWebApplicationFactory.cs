using Microsoft.AspNetCore.Mvc.Testing;

namespace Clinical.Test.IntegrationTests
{
    /// <summary>
    /// Boots the real API in-memory for end-to-end pipeline tests. Supplies the config the
    /// fail-fast startup requires (test-only values); no real database is contacted.
    /// </summary>
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public CustomWebApplicationFactory()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            Environment.SetEnvironmentVariable("JwtSettings__SecretKey",
                "IntegrationTestSigningKey-DoNotUseInProduction-0123456789");
            Environment.SetEnvironmentVariable("ConnectionStrings__ClinicalConnection",
                "Server=integration-test;Database=Clinical;Integrated Security=true;TrustServerCertificate=true");
        }
    }
}
