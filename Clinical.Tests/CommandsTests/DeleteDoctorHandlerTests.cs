using Clinical.Domain.Entities;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Exceptions;
using Clinical.UseCases.UseCases.Doctor.Commands.DeleteCommand;
using Moq;

namespace Clinical.Test.CommandsTests
{
    public class DeleteDoctorHandlerTests
    {
        private static (DeleteDoctorHandler handler, Mock<IGenericRepository<Doctor>> repo) Build()
        {
            var repo = new Mock<IGenericRepository<Doctor>>();
            var uow = new Mock<IUnitOfWork>();
            uow.Setup(u => u.Doctor).Returns(repo.Object);
            return (new DeleteDoctorHandler(uow.Object), repo);
        }

        [Fact]
        public async Task Handle_NoRowsAffected_ThrowsNotFound()
        {
            var (handler, repo) = Build();
            repo.Setup(r => r.ExecAsync(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(false);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(new DeleteDoctorCommand { DoctorId = 999 }, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_RowAffected_CompletesAndCallsRepository()
        {
            var (handler, repo) = Build();
            repo.Setup(r => r.ExecAsync(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(true);

            await handler.Handle(new DeleteDoctorCommand { DoctorId = 1 }, CancellationToken.None);

            repo.Verify(r => r.ExecAsync(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
        }
    }
}
