using Clinical.Application.DTOS.Exam.Response;

namespace Clinical.Interface.Interfaces
{
    public interface IExamRepository
    {
        Task<IEnumerable<GetAllExamResponseDto>> GetAllExam(string storedProcedure);
    }
}