using Clinical.Domain.Entities;
using Clinical.Infraestructure.Services;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.UseCases.Auth.Commands.LoginCommand;
using Clinical.Utils.Constants;
using Moq;

namespace Clinical.Test.AuthTests
{
    public class LoginHandlerTests
    {
        private static LoginHandler Build(
            Mock<IAuthRepository> authRepo,
            Mock<IJwtTokenService>? jwt = null,
            Mock<ILoginAttemptTracker>? attempts = null)
            => new(authRepo.Object, (jwt ?? new Mock<IJwtTokenService>()).Object,
                   (attempts ?? new Mock<ILoginAttemptTracker>()).Object);

        [Fact]
        public async Task Login_NonexistentUser_ReturnsGenericFailure_WithoutIssuingToken()
        {
            var authRepo = new Mock<IAuthRepository>();
            var jwt = new Mock<IJwtTokenService>();
            authRepo.Setup(a => a.GetUserByUsernameAsync(It.IsAny<string>())).ReturnsAsync((User?)null);

            var handler = Build(authRepo, jwt);
            var result = await handler.Handle(
                new LoginCommand { Username = "ghost", Password = "whatever" }, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(GlobalMessage.MESSAGE_TOKEN_ERROR, result.Message);
            jwt.Verify(j => j.GenerateAccessToken(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task Login_WrongPassword_ReturnsSameGenericFailure_AndRegistersFailure()
        {
            var authRepo = new Mock<IAuthRepository>();
            var attempts = new Mock<ILoginAttemptTracker>();
            authRepo.Setup(a => a.GetUserByUsernameAsync("alice")).ReturnsAsync(new User
            {
                UserId = 1,
                Username = "alice",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Correct123!"),
                State = 1
            });

            var handler = Build(authRepo, attempts: attempts);
            var result = await handler.Handle(
                new LoginCommand { Username = "alice", Password = "WrongPassword!" }, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(GlobalMessage.MESSAGE_TOKEN_ERROR, result.Message);
            attempts.Verify(a => a.RegisterFailure("alice"), Times.Once);
        }

        [Fact]
        public async Task Login_WhenLockedOut_ReturnsLockedMessage_WithoutHittingRepository()
        {
            var authRepo = new Mock<IAuthRepository>();
            var attempts = new Mock<ILoginAttemptTracker>();
            attempts.Setup(a => a.IsLockedOut("alice")).Returns(true);

            var handler = Build(authRepo, attempts: attempts);
            var result = await handler.Handle(
                new LoginCommand { Username = "alice", Password = "whatever" }, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(GlobalMessage.MESSAGE_ACCOUNT_LOCKED, result.Message);
            // Locked out short-circuits: no credential check at all.
            authRepo.Verify(a => a.GetUserByUsernameAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Login_Success_ResetsAttemptCounter()
        {
            var authRepo = new Mock<IAuthRepository>();
            var attempts = new Mock<ILoginAttemptTracker>();
            authRepo.Setup(a => a.GetUserByUsernameAsync("alice")).ReturnsAsync(new User
            {
                UserId = 1,
                Username = "alice",
                Email = "a@c.test",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Correct123!"),
                RoleId = 1,
                State = 1
            });
            authRepo.Setup(a => a.GetRoleNameAsync(1)).ReturnsAsync("Admin");
            var jwt = new Mock<IJwtTokenService>();
            jwt.Setup(j => j.GenerateAccessToken(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
               .Returns("token");
            jwt.Setup(j => j.GenerateRefreshToken()).Returns("refresh");

            var handler = Build(authRepo, jwt, attempts);
            var result = await handler.Handle(
                new LoginCommand { Username = "alice", Password = "Correct123!" }, CancellationToken.None);

            Assert.True(result.IsSuccess);
            attempts.Verify(a => a.Reset("alice"), Times.Once);
            attempts.Verify(a => a.RegisterFailure(It.IsAny<string>()), Times.Never);
        }
    }
}
