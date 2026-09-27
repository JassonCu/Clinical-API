using AutoMapper;
using Clinical.Application.DTOS.Analysis.Response;
using Clinical.Domain.Entities;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Exceptions;
using Clinical.UseCases.UseCases.Analysis.Queries.GetByIdQuery;
using Moq;

namespace Clinical.Test.HandlersTests
{
    public class AnalysisHandlerTests
    {
        private static Mock<IUnitOfWork> Uow(Mock<IGenericRepository<Analysis>> repo)
        {
            var uow = new Mock<IUnitOfWork>();
            uow.Setup(u => u.Analysis).Returns(repo.Object);
            return uow;
        }

        // Regression: a missing `return` previously made this return 200 with null Data instead of 404.
        [Fact]
        public async Task GetById_NotFound_ThrowsNotFound()
        {
            var repo = new Mock<IGenericRepository<Analysis>>();
            repo.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync((Analysis)null!);
            var handler = new GetAnalysisByIdHandler(Uow(repo).Object, new Mock<IMapper>().Object);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(new GetAnalysisByIdQuery { AnalysisId = 999 }, CancellationToken.None));
        }

        [Fact]
        public async Task GetById_Found_ReturnsMappedDto()
        {
            var repo = new Mock<IGenericRepository<Analysis>>();
            repo.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(new Analysis());
            var dto = new GetAnalysisByIdResponseDto();
            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<GetAnalysisByIdResponseDto>(It.IsAny<object>())).Returns(dto);
            var handler = new GetAnalysisByIdHandler(Uow(repo).Object, mapper.Object);

            var result = await handler.Handle(new GetAnalysisByIdQuery { AnalysisId = 1 }, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Same(dto, result.Data);
        }
    }
}
