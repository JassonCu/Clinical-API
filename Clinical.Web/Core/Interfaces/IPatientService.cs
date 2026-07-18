using Clinical.Web.Core.DTOs.Patient;

namespace Clinical.Web.Core.Interfaces;

public interface IPatientService
{
    Task<IEnumerable<PatientListDto>> GetAllAsync();
    Task<PatientDetailDto?> GetByIdAsync(int id);
    Task<bool> CreateAsync(CreatePatientDto dto);
    Task<bool> UpdateAsync(UpdatePatientDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> ChangeStateAsync(ChangeStatePatientDto dto);
}
