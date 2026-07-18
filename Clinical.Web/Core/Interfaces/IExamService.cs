using Clinical.Web.Core.DTOs.Exam;

namespace Clinical.Web.Core.Interfaces;

public interface IExamService
{
    Task<IEnumerable<ExamListDto>> GetAllAsync();
    Task<ExamDetailDto?> GetByIdAsync(int id);
    Task<bool> CreateAsync(CreateExamDto dto);
    Task<bool> UpdateAsync(UpdateExamDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> ChangeStateAsync(ChangeStateExamDto dto);
}
