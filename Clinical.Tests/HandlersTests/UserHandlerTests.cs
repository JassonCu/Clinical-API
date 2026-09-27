using Clinical.Application.DTOS.User.Request;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Exceptions;
using Clinical.UseCases.UseCases.User.Commands.ChangeStateCommand;
using Clinical.UseCases.UseCases.User.Commands.UpdateCommand;
using Moq;

namespace Clinical.Test.HandlersTests
{
    // Regression: these handlers used to return 204 even when the target user did not exist,
    // because the repository discarded the affected-rows count.
    public class UserHandlerTests
    {
        [Fact]
        public async Task UpdateUser_NoRowsAffected_ThrowsNotFound()
        {
            var repo = new Mock<IUserRepository>();
            repo.Setup(r => r.UpdateUserAsync(It.IsAny<UpdateUserDto>())).ReturnsAsync(0);
            var handler = new UpdateUserHandler(repo.Object);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(new UpdateUserCommand { UserId = 999 }, CancellationToken.None));
        }

        [Fact]
        public async Task UpdateUser_RowAffected_Succeeds()
        {
            var repo = new Mock<IUserRepository>();
            repo.Setup(r => r.UpdateUserAsync(It.IsAny<UpdateUserDto>())).ReturnsAsync(1);
            var handler = new UpdateUserHandler(repo.Object);

            var result = await handler.Handle(new UpdateUserCommand { UserId = 1 }, CancellationToken.None);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task ChangeStateUser_NoRowsAffected_ThrowsNotFound()
        {
            var repo = new Mock<IUserRepository>();
            repo.Setup(r => r.ChangeUserStateAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(0);
            var handler = new ChangeStateUserHandler(repo.Object);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(new ChangeStateUserCommand { UserId = 999, State = 0 }, CancellationToken.None));
        }

        [Fact]
        public async Task ChangeStateUser_RowAffected_Succeeds()
        {
            var repo = new Mock<IUserRepository>();
            repo.Setup(r => r.ChangeUserStateAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(1);
            var handler = new ChangeStateUserHandler(repo.Object);

            var result = await handler.Handle(new ChangeStateUserCommand { UserId = 1, State = 1 }, CancellationToken.None);

            Assert.True(result.IsSuccess);
        }
    }
}
