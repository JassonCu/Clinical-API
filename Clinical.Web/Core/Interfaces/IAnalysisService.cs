using Clinical.Web.Core.DTOs.Analysis;

namespace Clinical.Web.Core.Interfaces;

public interface IAnalysisService
{
    Task<IEnumerable<AnalysisListDto>> GetAllAsync();
    Task<bool> CreateAsync(CreateAnalysisDto dto);
    Task<bool> UpdateAsync(UpdateAnalysisDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> ChangeStateAsync(ChangeStateAnalysisDto dto);
}
