using Clinical.Web.Core.DTOs.ExamResult;
using Clinical.Web.Core.Interfaces;

namespace Clinical.Web.Infrastructure.Services;

public class ExamResultService : BaseApiService, IExamResultService
{
    public ExamResultService(IHttpClientFactory factory, IHttpContextAccessor accessor, ILogger<ExamResultService> logger)
        : base(factory, accessor, logger) { }

    public Task<IEnumerable<ExamResultListDto>> GetAllAsync() =>
        GetAsync<IEnumerable<ExamResultListDto>>("/api/examresult").ContinueWith(t => t.Result ?? []);

    public Task<ExamResultDetailDto?> GetByIdAsync(int id) => GetAsync<ExamResultDetailDto>($"/api/examresult/{id}");

    public Task<IEnumerable<ExamResultListDto>> GetByPatientAsync(int patientId) =>
        GetAsync<IEnumerable<ExamResultListDto>>($"/api/examresult/ByPatient/{patientId}").ContinueWith(t => t.Result ?? []);

    public Task<IEnumerable<ExamResultListDto>> GetByAppointmentAsync(int appointmentId) =>
        GetAsync<IEnumerable<ExamResultListDto>>($"/api/examresult/ByAppointment/{appointmentId}").ContinueWith(t => t.Result ?? []);

    public async Task<bool> CreateAsync(CreateExamResultDto dto) { var (ok, _) = await PostAsync("/api/examresult/Register", dto); return ok; }
    public async Task<bool> UpdateAsync(UpdateExamResultDto dto) { var (ok, _) = await PutAsync("/api/examresult/Edit", dto); return ok; }
    public async Task<bool> DeleteAsync(int id) { var (ok, _) = await DeleteAsync($"/api/examresult/Remove/{id}"); return ok; }
    public async Task<bool> ChangeStateAsync(ChangeStateExamResultDto dto) { var (ok, _) = await PatchAsync("/api/examresult/ChangeState", dto); return ok; }
}
