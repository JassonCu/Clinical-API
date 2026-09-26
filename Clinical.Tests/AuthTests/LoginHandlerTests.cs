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
        [Fact]
        public async Task Login_NonexistentUser_ReturnsGenericFailure_WithoutIssuingToken()
        {
            var authRepo = new Mock<IAuthRepository>();
            var jwt = new Mock<IJwtTokenService>();
            authRepo.Setup(a => a.GetUserByUsernameAsync(It.IsAny<string>())).ReturnsAsync((User?)null);

            var handler = new LoginHandler(authRepo.Object, jwt.Object);
            var result = await handler.Handle(
                new LoginCommand { Username = "ghost", Password = "whatever" }, CancellationToken.None);

            Assert.False(result.IsSuccess);
            // Same generic message as a wrong password → no username enumeration via the response.
            Assert.Equal(GlobalMessage.MESSAGE_TOKEN_ERROR, result.Message);
            jwt.Verify(j => j.GenerateAccessToken(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task Login_WrongPassword_ReturnsSameGenericFailure()
        {
            var authRepo = new Mock<IAuthRepository>();
            var jwt = new Mock<IJwtTokenService>();
            authRepo.Setup(a => a.GetUserByUsernameAsync("alice")).ReturnsAsync(new User
            {
                UserId = 1,
                Username = "alice",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Correct123!"),
                State = 1
            });

            var handler = new LoginHandler(authRepo.Object, jwt.Object);
            var result = await handler.Handle(
                new LoginCommand { Username = "alice", Password = "WrongPassword!" }, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(GlobalMessage.MESSAGE_TOKEN_ERROR, result.Message);
        }
    }
}
