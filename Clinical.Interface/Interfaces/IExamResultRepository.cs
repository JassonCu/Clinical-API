using Clinical.Application.DTOS.ExamResult.Response;

namespace Clinical.Interface.Interfaces
{
    public interface IExamResultRepository
    {
        Task<IEnumerable<GetAllExamResultResponseDto>> GetAllExamResults(string storedProcedure);
        Task<GetExamResultByIdResponseDto> GetExamResultById(string storedProcedure, object parameter);
        Task<IEnumerable<GetAllExamResultResponseDto>> GetExamResultsByPatient(string storedProcedure, object parameter);
        Task<IEnumerable<GetAllExamResultResponseDto>> GetExamResultsByAppointment(string storedProcedure, object parameter);
    }
}
