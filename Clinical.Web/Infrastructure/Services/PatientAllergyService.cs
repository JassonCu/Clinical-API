using Clinical.Web.Core.DTOs.PatientAllergy;
using Clinical.Web.Core.Interfaces;

namespace Clinical.Web.Infrastructure.Services;

public class PatientAllergyService : BaseApiService, IPatientAllergyService
{
    public PatientAllergyService(IHttpClientFactory factory, IHttpContextAccessor accessor, ILogger<PatientAllergyService> logger)
        : base(factory, accessor, logger) { }

    public Task<IEnumerable<AllergyDto>> GetAllAsync() =>
        GetAsync<IEnumerable<AllergyDto>>("/api/patientallergy").ContinueWith(t => t.Result ?? []);

    public Task<AllergyDto?> GetByIdAsync(int id) => GetAsync<AllergyDto>($"/api/patientallergy/{id}");

    public Task<IEnumerable<AllergyDto>> GetByPatientAsync(int patientId) =>
        GetAsync<IEnumerable<AllergyDto>>($"/api/patientallergy/ByPatient/{patientId}").ContinueWith(t => t.Result ?? []);

    public async Task<bool> CreateAsync(CreateAllergyDto dto) { var (ok, _) = await PostAsync("/api/patientallergy/Register", dto); return ok; }
    public async Task<bool> UpdateAsync(UpdateAllergyDto dto) { var (ok, _) = await PutAsync("/api/patientallergy/Edit", dto); return ok; }
    public async Task<bool> DeleteAsync(int id) { var (ok, _) = await DeleteAsync($"/api/patientallergy/Remove/{id}"); return ok; }
    public async Task<bool> ChangeStateAsync(ChangeStateAllergyDto dto) { var (ok, _) = await PatchAsync("/api/patientallergy/ChangeState", dto); return ok; }
}
