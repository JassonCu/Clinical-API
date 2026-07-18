using Clinical.Web.Core.DTOs.Medicine;

namespace Clinical.Web.Core.Interfaces;

public interface IMedicineService
{
    Task<IEnumerable<MedicineListDto>> GetAllAsync();
    Task<MedicineDetailDto?> GetByIdAsync(int id);
    Task<IEnumerable<MedicineListDto>> GetLowStockAsync();
    Task<bool> CreateAsync(CreateMedicineDto dto);
    Task<bool> UpdateAsync(UpdateMedicineDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> ChangeStateAsync(ChangeStateMedicineDto dto);
}
