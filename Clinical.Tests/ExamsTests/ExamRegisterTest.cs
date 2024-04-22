using Clinical.API.Controllers;
using Clinical.Application.DTOS.Exam.Response;
using Clinical.UseCases.Commons.Bases;
using Clinical.UseCases.UseCases.Exam.Queries.GetAllQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Clinical.Test
{
    public class ExamControllerTests
    {
        [Fact]
        public async Task ListExams_Returns_OkResult_With_Exams()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var response = new BaseResponse<IEnumerable<GetAllExamResponseDto>>();
            response.Data = new List<GetAllExamResponseDto>();

            mediatorMock.Setup(m => m.Send(It.IsAny<GetAllExamQuery>(), default))
                        .ReturnsAsync(response);

            var controller = new ExamController(mediatorMock.Object);

            // Act
            var result = await controller.ListExams();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseAssert = Assert.IsAssignableFrom<BaseResponse<IEnumerable<GetAllExamResponseDto>>>(okResult.Value);
            var model = responseAssert.Data;
            Assert.Empty(model);
        }

    }
}
