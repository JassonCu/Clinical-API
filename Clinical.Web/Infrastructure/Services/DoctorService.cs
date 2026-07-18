using Clinical.Web.Core.DTOs.Doctor;
using Clinical.Web.Core.Interfaces;

namespace Clinical.Web.Infrastructure.Services;

public class DoctorService : BaseApiService, IDoctorService
{
    public DoctorService(IHttpClientFactory factory, IHttpContextAccessor accessor, ILogger<DoctorService> logger)
        : base(factory, accessor, logger) { }

    public Task<IEnumerable<DoctorListDto>> GetAllAsync() =>
        GetAsync<IEnumerable<DoctorListDto>>("/api/doctor").ContinueWith(t => t.Result ?? []);

    public Task<DoctorDetailDto?> GetByIdAsync(int id) =>
        GetAsync<DoctorDetailDto>($"/api/doctor/{id}");

    public async Task<bool> CreateAsync(CreateDoctorDto dto) { var (ok, _) = await PostAsync("/api/doctor/Register", dto); return ok; }
    public async Task<bool> UpdateAsync(UpdateDoctorDto dto) { var (ok, _) = await PutAsync("/api/doctor/Edit", dto); return ok; }
    public async Task<bool> DeleteAsync(int id) { var (ok, _) = await DeleteAsync($"/api/doctor/Remove/{id}"); return ok; }
    public async Task<bool> ChangeStateAsync(ChangeStateDoctorDto dto) { var (ok, _) = await PatchAsync("/api/doctor/ChangeState", dto); return ok; }
}
