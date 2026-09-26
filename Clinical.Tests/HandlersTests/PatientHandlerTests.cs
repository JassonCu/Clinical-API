using AutoMapper;
using Clinical.Application.DTOS.Patient.Response;
using Clinical.Domain.Entities;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Exceptions;
using Clinical.UseCases.UseCases.Patient.Commands.DeleteCommand;
using Clinical.UseCases.UseCases.Patient.Queries.GetByIdQuery;
using Moq;

namespace Clinical.Test.HandlersTests
{
    public class PatientHandlerTests
    {
        private static Mock<IUnitOfWork> UowWithPatientRepo(Mock<IGenericRepository<Patient>> repo)
        {
            var uow = new Mock<IUnitOfWork>();
            uow.Setup(u => u.Patient).Returns(repo.Object);
            return uow;
        }

        [Fact]
        public async Task Delete_NoRowsAffected_ThrowsNotFound()
        {
            var repo = new Mock<IGenericRepository<Patient>>();
            repo.Setup(r => r.ExecAsync(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(false);
            var handler = new DeletePatientHandler(UowWithPatientRepo(repo).Object);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(new DeletePatientCommand { PatientId = 999 }, CancellationToken.None));
        }

        [Fact]
        public async Task Delete_RowAffected_Succeeds()
        {
            var repo = new Mock<IGenericRepository<Patient>>();
            repo.Setup(r => r.ExecAsync(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(true);
            var handler = new DeletePatientHandler(UowWithPatientRepo(repo).Object);

            var result = await handler.Handle(new DeletePatientCommand { PatientId = 1 }, CancellationToken.None);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task GetById_NotFound_ThrowsNotFound()
        {
            var repo = new Mock<IGenericRepository<Patient>>();
            repo.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync((Patient)null!);
            var handler = new GetPatientByIdHandler(UowWithPatientRepo(repo).Object, new Mock<IMapper>().Object);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(new GetPatientByIdQuery { PatientId = 999 }, CancellationToken.None));
        }

        [Fact]
        public async Task GetById_Found_ReturnsMappedDto()
        {
            var repo = new Mock<IGenericRepository<Patient>>();
            repo.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(new Patient());
            var dto = new GetPatientByIdResponseDto();
            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<GetPatientByIdResponseDto>(It.IsAny<object>())).Returns(dto);

            var handler = new GetPatientByIdHandler(UowWithPatientRepo(repo).Object, mapper.Object);
            var result = await handler.Handle(new GetPatientByIdQuery { PatientId = 1 }, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Same(dto, result.Data);
        }
    }
}
