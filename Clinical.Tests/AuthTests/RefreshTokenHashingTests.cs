using Clinical.Domain.Entities;
using Clinical.Infraestructure.Services;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.UseCases.Auth.Commands.LoginCommand;
using Clinical.UseCases.UseCases.Auth.Commands.RefreshTokenCommand;
using Clinical.Utils.Security;
using Moq;

namespace Clinical.Test.AuthTests
{
    public class RefreshTokenHashingTests
    {
        [Fact]
        public async Task Login_StoresHashedRefreshToken_ButReturnsRawToRawClient()
        {
            const string rawRefresh = "RAW-REFRESH-TOKEN-value";
            var authRepo = new Mock<IAuthRepository>();
            var jwt = new Mock<IJwtTokenService>();

            authRepo.Setup(a => a.GetUserByUsernameAsync("alice")).ReturnsAsync(new User
            {
                UserId = 7,
                Username = "alice",
                Email = "alice@clinic.test",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Secret123!"),
                RoleId = 1,
                State = 1
            });
            authRepo.Setup(a => a.GetRoleNameAsync(1)).ReturnsAsync("Admin");
            jwt.Setup(j => j.GenerateAccessToken(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
               .Returns("access-token");
            jwt.Setup(j => j.GenerateRefreshToken()).Returns(rawRefresh);

            var handler = new LoginHandler(authRepo.Object, jwt.Object);
            var result = await handler.Handle(new LoginCommand { Username = "alice", Password = "Secret123!" }, CancellationToken.None);

            Assert.True(result.IsSuccess);
            // Client receives the RAW token...
            Assert.Equal(rawRefresh, result.Data!.RefreshToken);
            // ...but the DB only ever sees the HASH.
            authRepo.Verify(a => a.UpdateRefreshTokenAsync(7, TokenHasher.Hash(rawRefresh), It.IsAny<DateTime>()), Times.Once);
            authRepo.Verify(a => a.UpdateRefreshTokenAsync(It.IsAny<int>(), rawRefresh, It.IsAny<DateTime>()), Times.Never);
        }

        [Fact]
        public async Task Refresh_LooksUpByHash_NotRawToken()
        {
            const string incomingRaw = "CLIENT-HELD-REFRESH-TOKEN";
            var authRepo = new Mock<IAuthRepository>();
            var jwt = new Mock<IJwtTokenService>();

            authRepo.Setup(a => a.GetUserByRefreshTokenAsync(TokenHasher.Hash(incomingRaw))).ReturnsAsync(new User
            {
                UserId = 7,
                Username = "alice",
                Email = "alice@clinic.test",
                RoleId = 1,
                RefreshTokenExpiry = DateTime.UtcNow.AddDays(3)
            });
            authRepo.Setup(a => a.GetRoleNameAsync(1)).ReturnsAsync("Admin");
            jwt.Setup(j => j.GenerateAccessToken(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
               .Returns("new-access");
            jwt.Setup(j => j.GenerateRefreshToken()).Returns("NEW-RAW");

            var handler = new RefreshTokenHandler(authRepo.Object, jwt.Object);
            var result = await handler.Handle(new RefreshTokenCommand { RefreshToken = incomingRaw }, CancellationToken.None);

            Assert.True(result.IsSuccess);
            // Lookup used the hash, never the raw token.
            authRepo.Verify(a => a.GetUserByRefreshTokenAsync(TokenHasher.Hash(incomingRaw)), Times.Once);
            authRepo.Verify(a => a.GetUserByRefreshTokenAsync(incomingRaw), Times.Never);
            // Rotation stores a hash and returns the new raw token.
            authRepo.Verify(a => a.UpdateRefreshTokenAsync(7, TokenHasher.Hash("NEW-RAW"), It.IsAny<DateTime>()), Times.Once);
            Assert.Equal("NEW-RAW", result.Data!.RefreshToken);
        }
    }
}
