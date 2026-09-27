using System.Net;
using System.Net.Http.Json;
using Clinical.Domain.Entities;
using Clinical.Interface.Interfaces;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Clinical.Test.IntegrationTests
{
    public class ApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public ApiIntegrationTests(CustomWebApplicationFactory factory) => _factory = factory;

        [Fact]
        public async Task ProtectedEndpoint_WithoutToken_Returns401()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/patient");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task UnknownRoute_Returns404()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/does-not-exist");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task SecurityHeaders_ArePresentOnResponses()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/patient");
            Assert.True(response.Headers.Contains("X-Content-Type-Options"));
            Assert.True(response.Headers.Contains("X-Frame-Options"));
        }

        [Fact]
        public async Task Login_InvalidBody_Returns400_ProblemJson_WithFieldErrors()
        {
            var client = _factory.CreateClient();
            var response = await client.PostAsJsonAsync("/api/auth/Login", new { });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
            var body = await response.Content.ReadAsStringAsync();
            Assert.Contains("\"errors\"", body);      // ValidationProblemDetails shape
            Assert.Contains("\"traceId\"", body);     // our middleware enrichment
        }

        [Fact]
        public async Task Login_WrongCredentials_Returns401()
        {
            var client = ClientWithAuthRepo(repo =>
                repo.Setup(r => r.GetUserByUsernameAsync(It.IsAny<string>())).ReturnsAsync(new User
                {
                    UserId = 1,
                    Username = "alice",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Right123!"),
                    State = 1
                }));

            var response = await client.PostAsJsonAsync("/api/auth/Login",
                new { Username = "alice", Password = "WrongPassword!" });

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Login_ValidCredentials_Returns200_WithTokens()
        {
            var client = ClientWithAuthRepo(repo =>
            {
                repo.Setup(r => r.GetUserByUsernameAsync("alice")).ReturnsAsync(new User
                {
                    UserId = 1,
                    Username = "alice",
                    Email = "alice@clinic.test",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Right123!"),
                    RoleId = 1,
                    State = 1
                });
                repo.Setup(r => r.GetRoleNameAsync(1)).ReturnsAsync("Admin");
                repo.Setup(r => r.UpdateRefreshTokenAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                    .ReturnsAsync(true);
            });

            var response = await client.PostAsJsonAsync("/api/auth/Login",
                new { Username = "alice", Password = "Right123!" });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadAsStringAsync();
            Assert.Contains("accessToken", body);
            Assert.Contains("refreshToken", body);
        }

        // Spins up an isolated server whose IAuthRepository is a mock, so the full HTTP pipeline
        // (validation, MediatR, handler, ProblemDetails, JSON) runs without a real database.
        private HttpClient ClientWithAuthRepo(Action<Mock<IAuthRepository>> configure)
        {
            var repo = new Mock<IAuthRepository>();
            configure(repo);
            return _factory.WithWebHostBuilder(builder =>
                builder.ConfigureTestServices(services => services.AddScoped(_ => repo.Object)))
                .CreateClient();
        }
    }
}
