using Clinical.Domain.Entities;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Exceptions;
using Clinical.UseCases.UseCases.Auth.Commands.RegisterCommand;
using Moq;

namespace Clinical.Test.AuthTests
{
    public class RegisterHandlerTests
    {
        private static RegisterCommand NewCommand(string username) => new()
        {
            Username = username,
            Email = "user@clinic.test",
            Password = "Passw0rd!",
            FirstName = "New",
            LastName = "User",
            RoleId = 2
        };

        [Fact]
        public async Task Handle_ExistingUsername_ThrowsConflict_AndDoesNotRegister()
        {
            var authRepo = new Mock<IAuthRepository>();
            authRepo.Setup(a => a.GetUserByUsernameAsync("bob"))
                    .ReturnsAsync(new User { UserId = 1, Username = "bob" });

            var handler = new RegisterHandler(authRepo.Object);

            await Assert.ThrowsAsync<ConflictException>(() =>
                handler.Handle(NewCommand("bob"), CancellationToken.None));

            authRepo.Verify(a => a.RegisterUserAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task Handle_NewUsername_RegistersSuccessfully()
        {
            var authRepo = new Mock<IAuthRepository>();
            authRepo.Setup(a => a.GetUserByUsernameAsync("newuser")).ReturnsAsync((User?)null);
            authRepo.Setup(a => a.RegisterUserAsync(It.IsAny<User>())).ReturnsAsync(true);

            var handler = new RegisterHandler(authRepo.Object);

            var result = await handler.Handle(NewCommand("newuser"), CancellationToken.None);

            Assert.True(result.IsSuccess);
        }
    }
}
