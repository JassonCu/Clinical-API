using Clinical.Application.DTOS.User.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Exceptions;
using Clinical.UseCases.UseCases.User.Commands.GenerateResetTokenCommand;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Clinical.Test.HandlersTests
{
    public class GenerateResetTokenHandlerTests
    {
        private static GenerateResetTokenHandler Build(
            Mock<IPasswordResetRepository> resetRepo,
            Mock<IPasswordResetNotifier> notifier,
            UserDetailDto? user)
        {
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(u => u.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(user);
            return new GenerateResetTokenHandler(
                resetRepo.Object, userRepo.Object, notifier.Object,
                NullLogger<GenerateResetTokenHandler>.Instance);
        }

        private static UserDetailDto SampleUser() =>
            new() { UserId = 5, Username = "bob", Email = "bob@clinic.test" };

        [Fact]
        public async Task Handle_UserNotFound_ThrowsNotFound()
        {
            var handler = Build(new Mock<IPasswordResetRepository>(), new Mock<IPasswordResetNotifier>(), user: null);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(new GenerateResetTokenCommand { UserId = 999 }, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_NoDeliveryChannel_ReturnsRawToken_AndStoresHash()
        {
            var resetRepo = new Mock<IPasswordResetRepository>();
            var notifier = new Mock<IPasswordResetNotifier>();
            notifier.Setup(n => n.TryNotifyAsync(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<string>(),
                It.IsAny<DateTime>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

            var handler = Build(resetRepo, notifier, SampleUser());
            var result = await handler.Handle(new GenerateResetTokenCommand { UserId = 5 }, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.False(string.IsNullOrEmpty(result.Data!.RawToken));   // returned for manual sharing
            // The stored value is a SHA-256 hex hash (64 chars), never the raw token.
            resetRepo.Verify(r => r.CreateTokenAsync(5,
                It.Is<string>(h => h.Length == 64 && h != result.Data.RawToken), It.IsAny<DateTime>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenDeliveredDirectly_DoesNotReturnRawToken()
        {
            var notifier = new Mock<IPasswordResetNotifier>();
            notifier.Setup(n => n.TryNotifyAsync(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<string>(),
                It.IsAny<DateTime>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var handler = Build(new Mock<IPasswordResetRepository>(), notifier, SampleUser());
            var result = await handler.Handle(new GenerateResetTokenCommand { UserId = 5 }, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Null(result.Data!.RawToken);   // delivered to the user → not exposed to the caller
        }
    }
}
