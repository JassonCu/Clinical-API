using Clinical.Domain.Entities;

namespace Clinical.Interface.Interfaces
{
    public interface IAnalysisRepository
    {
        Task<IEnumerable<Analysis>> ListAnalysis();
        Task<Analysis> AnalysisById(int analysisId);
        Task<bool> AnalysisRegister(Analysis analysis);
        Task<bool> AnalysisEdit(Analysis analysis);
        Task<bool> AnalysisRemove(int analysisId);
    }

}
