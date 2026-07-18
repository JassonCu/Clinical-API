using Clinical.Web.Core.DTOs.Exam;
using Clinical.Web.Core.Interfaces;

namespace Clinical.Web.Infrastructure.Services;

public class ExamService : BaseApiService, IExamService
{
    public ExamService(IHttpClientFactory factory, IHttpContextAccessor accessor, ILogger<ExamService> logger)
        : base(factory, accessor, logger) { }

    public Task<IEnumerable<ExamListDto>> GetAllAsync() =>
        GetAsync<IEnumerable<ExamListDto>>("/api/exam").ContinueWith(t => t.Result ?? []);

    public Task<ExamDetailDto?> GetByIdAsync(int id) => GetAsync<ExamDetailDto>($"/api/exam/{id}");

    public async Task<bool> CreateAsync(CreateExamDto dto) { var (ok, _) = await PostAsync("/api/exam/Register", dto); return ok; }
    public async Task<bool> UpdateAsync(UpdateExamDto dto) { var (ok, _) = await PutAsync("/api/exam/Edit", dto); return ok; }
    public async Task<bool> DeleteAsync(int id) { var (ok, _) = await DeleteAsync($"/api/exam/Remove/{id}"); return ok; }
    public async Task<bool> ChangeStateAsync(ChangeStateExamDto dto) { var (ok, _) = await PatchAsync("/api/exam/ChangeState", dto); return ok; }
}
