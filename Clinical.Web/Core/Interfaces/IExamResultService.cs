using Clinical.Web.Core.DTOs.ExamResult;

namespace Clinical.Web.Core.Interfaces;

public interface IExamResultService
{
    Task<IEnumerable<ExamResultListDto>> GetAllAsync();
    Task<ExamResultDetailDto?> GetByIdAsync(int id);
    Task<IEnumerable<ExamResultListDto>> GetByPatientAsync(int patientId);
    Task<IEnumerable<ExamResultListDto>> GetByAppointmentAsync(int appointmentId);
    Task<bool> CreateAsync(CreateExamResultDto dto);
    Task<bool> UpdateAsync(UpdateExamResultDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> ChangeStateAsync(ChangeStateExamResultDto dto);
}
