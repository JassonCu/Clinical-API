using AutoMapper;
using Clinical.Domain.Entities;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Exceptions;
using Clinical.UseCases.UseCases.Doctor.Commands.UpdateCommand;
using Moq;

namespace Clinical.Test.HandlersTests
{
    // Doctor was migrated to the "Stage 2" shape: the handler returns Task and throws on failure,
    // instead of returning a BaseResponse envelope.
    public class DoctorUpdateHandlerTests
    {
        private static (UpdateDoctorHandler handler, Mock<IGenericRepository<Doctor>> repo) Build()
        {
            var repo = new Mock<IGenericRepository<Doctor>>();
            var uow = new Mock<IUnitOfWork>();
            uow.Setup(u => u.Doctor).Returns(repo.Object);
            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<Doctor>(It.IsAny<object>())).Returns(new Doctor { DoctorId = 1, FirstName = "x" });
            return (new UpdateDoctorHandler(uow.Object, mapper.Object), repo);
        }

        [Fact]
        public async Task Update_NoRowsAffected_ThrowsNotFound()
        {
            var (handler, repo) = Build();
            repo.Setup(r => r.ExecAsync(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(false);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(new UpdateDoctorCommand { DoctorId = 999 }, CancellationToken.None));
        }

        [Fact]
        public async Task Update_RowAffected_CompletesAndCallsRepository()
        {
            var (handler, repo) = Build();
            repo.Setup(r => r.ExecAsync(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(true);

            await handler.Handle(new UpdateDoctorCommand { DoctorId = 1, FirstName = "Nuevo" }, CancellationToken.None);

            repo.Verify(r => r.ExecAsync(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
        }
    }
}
