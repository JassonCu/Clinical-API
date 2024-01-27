using Clinical.Domain.Entities;

namespace Clinical.Interface
{
    public interface IAnalysisRepository
    {
        Task<IEnumerable<Analysis>> ListAnalysis();
    }

}
