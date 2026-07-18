using Clinical.Web.Core.DTOs.Medicine;
using Clinical.Web.Core.Interfaces;

namespace Clinical.Web.Infrastructure.Services;

public class MedicineService : BaseApiService, IMedicineService
{
    public MedicineService(IHttpClientFactory factory, IHttpContextAccessor accessor, ILogger<MedicineService> logger)
        : base(factory, accessor, logger) { }

    public Task<IEnumerable<MedicineListDto>> GetAllAsync() =>
        GetAsync<IEnumerable<MedicineListDto>>("/api/medicine").ContinueWith(t => t.Result ?? []);

    public Task<MedicineDetailDto?> GetByIdAsync(int id) =>
        GetAsync<MedicineDetailDto>($"/api/medicine/{id}");

    public Task<IEnumerable<MedicineListDto>> GetLowStockAsync() =>
        GetAsync<IEnumerable<MedicineListDto>>("/api/medicine/LowStock").ContinueWith(t => t.Result ?? []);

    public async Task<bool> CreateAsync(CreateMedicineDto dto) { var (ok, _) = await PostAsync("/api/medicine/Register", dto); return ok; }
    public async Task<bool> UpdateAsync(UpdateMedicineDto dto) { var (ok, _) = await PutAsync("/api/medicine/Edit", dto); return ok; }
    public async Task<bool> DeleteAsync(int id) { var (ok, _) = await DeleteAsync($"/api/medicine/Remove/{id}"); return ok; }
    public async Task<bool> ChangeStateAsync(ChangeStateMedicineDto dto) { var (ok, _) = await PatchAsync("/api/medicine/ChangeState", dto); return ok; }
}
