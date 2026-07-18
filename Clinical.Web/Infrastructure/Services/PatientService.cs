using Clinical.Web.Core.DTOs.Patient;
using Clinical.Web.Core.Interfaces;

namespace Clinical.Web.Infrastructure.Services;

public class PatientService : BaseApiService, IPatientService
{
    public PatientService(IHttpClientFactory factory, IHttpContextAccessor accessor, ILogger<PatientService> logger)
        : base(factory, accessor, logger) { }

    public Task<IEnumerable<PatientListDto>> GetAllAsync() =>
        GetAsync<IEnumerable<PatientListDto>>("/api/patient").ContinueWith(t => t.Result ?? []);

    public Task<PatientDetailDto?> GetByIdAsync(int id) =>
        GetAsync<PatientDetailDto>($"/api/patient/{id}");

    public async Task<bool> CreateAsync(CreatePatientDto dto)
    {
        var (ok, _) = await PostAsync("/api/patient/Register", dto);
        return ok;
    }

    public async Task<bool> UpdateAsync(UpdatePatientDto dto)
    {
        var (ok, _) = await PutAsync("/api/patient/Edit", dto);
        return ok;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var (ok, _) = await DeleteAsync($"/api/patient/Remove/{id}");
        return ok;
    }

    public async Task<bool> ChangeStateAsync(ChangeStatePatientDto dto)
    {
        var (ok, _) = await PatchAsync("/api/patient/ChangeState", dto);
        return ok;
    }
}
