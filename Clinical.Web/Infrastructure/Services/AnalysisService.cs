using Clinical.Web.Core.DTOs.Analysis;
using Clinical.Web.Core.Interfaces;

namespace Clinical.Web.Infrastructure.Services;

public class AnalysisService : BaseApiService, IAnalysisService
{
    public AnalysisService(IHttpClientFactory factory, IHttpContextAccessor accessor, ILogger<AnalysisService> logger)
        : base(factory, accessor, logger) { }

    public Task<IEnumerable<AnalysisListDto>> GetAllAsync() =>
        GetAsync<IEnumerable<AnalysisListDto>>("/api/analysis").ContinueWith(t => t.Result ?? []);

    public async Task<bool> CreateAsync(CreateAnalysisDto dto) { var (ok, _) = await PostAsync("/api/analysis/Register", dto); return ok; }
    public async Task<bool> UpdateAsync(UpdateAnalysisDto dto) { var (ok, _) = await PutAsync("/api/analysis/Edit", dto); return ok; }
    public async Task<bool> DeleteAsync(int id) { var (ok, _) = await DeleteAsync($"/api/analysis/Remove/{id}"); return ok; }
    public async Task<bool> ChangeStateAsync(ChangeStateAnalysisDto dto) { var (ok, _) = await PatchAsync("/api/analysis/ChangeState", dto); return ok; }
}
