using Clinical.Web.Core.DTOs.Prescription;
using Clinical.Web.Core.Interfaces;

namespace Clinical.Web.Infrastructure.Services;

public class PrescriptionService : BaseApiService, IPrescriptionService
{
    public PrescriptionService(IHttpClientFactory factory, IHttpContextAccessor accessor, ILogger<PrescriptionService> logger)
        : base(factory, accessor, logger) { }

    public Task<IEnumerable<PrescriptionListDto>> GetAllAsync() =>
        GetAsync<IEnumerable<PrescriptionListDto>>("/api/prescription").ContinueWith(t => t.Result ?? []);

    public Task<PrescriptionDetailDto?> GetByIdAsync(int id) =>
        GetAsync<PrescriptionDetailDto>($"/api/prescription/{id}");

    public Task<IEnumerable<PrescriptionListDto>> GetByPatientAsync(int patientId) =>
        GetAsync<IEnumerable<PrescriptionListDto>>($"/api/prescription/ByPatient/{patientId}").ContinueWith(t => t.Result ?? []);

    public Task<IEnumerable<PrescriptionListDto>> GetByDoctorAsync(int doctorId) =>
        GetAsync<IEnumerable<PrescriptionListDto>>($"/api/prescription/ByDoctor/{doctorId}").ContinueWith(t => t.Result ?? []);

    public async Task<bool> CreateAsync(CreatePrescriptionDto dto) { var (ok, _) = await PostAsync("/api/prescription/Register", dto); return ok; }
    public async Task<bool> DeleteAsync(int id) { var (ok, _) = await DeleteAsync($"/api/prescription/Remove/{id}"); return ok; }
    public async Task<bool> ChangeStateAsync(ChangeStatePrescriptionDto dto) { var (ok, _) = await PatchAsync("/api/prescription/ChangeState", dto); return ok; }
}
