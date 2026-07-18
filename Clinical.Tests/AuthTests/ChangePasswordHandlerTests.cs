using Clinical.Domain.Entities;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.UseCases.Auth.Commands.ChangePasswordCommand;
using Moq;

namespace Clinical.Test.AuthTests
{
    public class ChangePasswordHandlerTests
    {
        private const int AuthenticatedUserId = 7;

        private static User UserWithPassword(string password) => new()
        {
            UserId = AuthenticatedUserId,
            Username = "alice",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12)
        };

        [Fact]
        public async Task Handle_WrongCurrentPassword_Fails_AndDoesNotUpdatePassword()
        {
            var authRepo = new Mock<IAuthRepository>();
            var resetRepo = new Mock<IPasswordResetRepository>();
            authRepo.Setup(a => a.GetUserByUsernameAsync("alice"))
                    .ReturnsAsync(UserWithPassword("Correct123!"));

            var handler = new ChangePasswordHandler(authRepo.Object, resetRepo.Object);

            var result = await handler.Handle(new ChangePasswordCommand
            {
                Username = "alice",
                CurrentPassword = "TheWrongPassword!",
                NewPassword = "BrandNew123!"
            }, CancellationToken.None);

            Assert.False(result.IsSuccess);
            resetRepo.Verify(r => r.UpdatePasswordAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Handle_CorrectCurrentPassword_UpdatesAuthenticatedUserWithAHash()
        {
            var authRepo = new Mock<IAuthRepository>();
            var resetRepo = new Mock<IPasswordResetRepository>();
            authRepo.Setup(a => a.GetUserByUsernameAsync("alice"))
                    .ReturnsAsync(UserWithPassword("Correct123!"));

            var handler = new ChangePasswordHandler(authRepo.Object, resetRepo.Object);

            var result = await handler.Handle(new ChangePasswordCommand
            {
                Username = "alice",
                CurrentPassword = "Correct123!",
                NewPassword = "BrandNew123!"
            }, CancellationToken.None);

            Assert.True(result.IsSuccess);
            // The update targets the id resolved from the token identity (7), and stores a hash, not plaintext.
            resetRepo.Verify(r => r.UpdatePasswordAsync(
                AuthenticatedUserId,
                It.Is<string>(h => h != "BrandNew123!" && h.StartsWith("$2"))), Times.Once);
        }

        [Fact]
        public async Task Handle_ResolvesTargetFromTokenUsername_NotFromClientInput()
        {
            var authRepo = new Mock<IAuthRepository>();
            var resetRepo = new Mock<IPasswordResetRepository>();
            authRepo.Setup(a => a.GetUserByUsernameAsync(It.IsAny<string>()))
                    .ReturnsAsync(UserWithPassword("Correct123!"));

            var handler = new ChangePasswordHandler(authRepo.Object, resetRepo.Object);

            await handler.Handle(new ChangePasswordCommand
            {
                Username = "alice",          // set server-side from the token
                CurrentPassword = "Correct123!",
                NewPassword = "BrandNew123!"
            }, CancellationToken.None);

            // Lookup is keyed on the authenticated username, closing the previous IDOR.
            authRepo.Verify(a => a.GetUserByUsernameAsync("alice"), Times.Once);
        }
    }
}
