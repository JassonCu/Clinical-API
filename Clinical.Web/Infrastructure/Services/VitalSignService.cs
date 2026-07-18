using Clinical.Web.Core.DTOs.VitalSign;
using Clinical.Web.Core.Interfaces;

namespace Clinical.Web.Infrastructure.Services;

public class VitalSignService : BaseApiService, IVitalSignService
{
    public VitalSignService(IHttpClientFactory factory, IHttpContextAccessor accessor, ILogger<VitalSignService> logger)
        : base(factory, accessor, logger) { }

    public Task<IEnumerable<VitalSignDto>> GetAllAsync() =>
        GetAsync<IEnumerable<VitalSignDto>>("/api/vitalsign").ContinueWith(t => t.Result ?? []);

    public Task<IEnumerable<VitalSignDto>> GetByPatientAsync(int patientId) =>
        GetAsync<IEnumerable<VitalSignDto>>($"/api/vitalsign/ByPatient/{patientId}").ContinueWith(t => t.Result ?? []);

    public async Task<bool> CreateAsync(CreateVitalSignDto dto) { var (ok, _) = await PostAsync("/api/vitalsign/Register", dto); return ok; }
    public async Task<bool> DeleteAsync(int id) { var (ok, _) = await DeleteAsync($"/api/vitalsign/Remove/{id}"); return ok; }
}
