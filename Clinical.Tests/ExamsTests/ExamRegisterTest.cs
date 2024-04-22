using Clinical.API.Controllers;
using Clinical.Application.DTOS.Exam.Response;
using Clinical.UseCases.Commons.Bases;
using Clinical.UseCases.UseCases.Exam.Queries.GetAllQuery;
using Clinical.UseCases.UseCases.Exam.Queries.GetByIdQuery;
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

        [Fact]
        public async Task ExamById_Returns_OkResult_With_Exam()
        {
            // Arrange
            int examId = 1;
            var expectedExam = new GetExamByIdResponseDto
            {
                ExamId = examId,
                Name = "Nombre del examen",
            };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<GetExamByIdQuery>(), default))
                        .ReturnsAsync(new BaseResponse<GetExamByIdResponseDto> { Data = expectedExam });

            var controller = new ExamController(mediatorMock.Object);

            // Act
            var result = await controller.ExamById(examId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<BaseResponse<GetExamByIdResponseDto>>(okResult.Value);
            var model = Assert.IsAssignableFrom<GetExamByIdResponseDto>(response.Data);
            Assert.Equal(expectedExam.ExamId, model.ExamId);
            Assert.Equal(expectedExam.Name, model.Name);
        }

    }
}
