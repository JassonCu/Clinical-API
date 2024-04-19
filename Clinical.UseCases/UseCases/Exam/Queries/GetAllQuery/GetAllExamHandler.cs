using Clinical.Application.DTOS.Exam.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.Exam.Queries.GetAllQuery
{
    public class GetAllExamHandler : IRequestHandler<GetAllExamQuery, BaseResponse<IEnumerable<GetAllExamResponseDto>>>
    {
        private readonly IExamRepository _examRepository;

        public GetAllExamHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<BaseResponse<IEnumerable<GetAllExamResponseDto>>> Handle(GetAllExamQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<IEnumerable<GetAllExamResponseDto>>();

            try
            {
                var exams = await _examRepository.GetAllExam(StoreProcedures.uspExamList);

                if (exams is not null)
                {
                    response.IsSuccess = true;
                    response.Data = exams;
                    response.Message = "Consulta exitosa";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
