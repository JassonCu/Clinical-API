using Clinical.Web.Core.DTOs.MedicalHistory;
using Clinical.Web.Core.Interfaces;

namespace Clinical.Web.Infrastructure.Services;

public class MedicalHistoryService : BaseApiService, IMedicalHistoryService
{
    public MedicalHistoryService(IHttpClientFactory factory, IHttpContextAccessor accessor, ILogger<MedicalHistoryService> logger)
        : base(factory, accessor, logger) { }

    public Task<MedicalHistoryDto?> GetByIdAsync(int id) => GetAsync<MedicalHistoryDto>($"/api/medicalhistory/{id}");

    public Task<IEnumerable<MedicalHistoryDto>> GetByPatientAsync(int patientId) =>
        GetAsync<IEnumerable<MedicalHistoryDto>>($"/api/medicalhistory/ByPatient/{patientId}").ContinueWith(t => t.Result ?? []);

    public async Task<bool> CreateAsync(CreateMedicalHistoryDto dto) { var (ok, _) = await PostAsync("/api/medicalhistory/Register", dto); return ok; }
    public async Task<bool> UpdateAsync(UpdateMedicalHistoryDto dto) { var (ok, _) = await PutAsync("/api/medicalhistory/Edit", dto); return ok; }
    public async Task<bool> DeleteAsync(int id) { var (ok, _) = await DeleteAsync($"/api/medicalhistory/Remove/{id}"); return ok; }
}
