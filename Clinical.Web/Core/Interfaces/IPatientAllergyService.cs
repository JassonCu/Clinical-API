using Clinical.Web.Core.DTOs.PatientAllergy;

namespace Clinical.Web.Core.Interfaces;

public interface IPatientAllergyService
{
    Task<IEnumerable<AllergyDto>> GetAllAsync();
    Task<AllergyDto?> GetByIdAsync(int id);
    Task<IEnumerable<AllergyDto>> GetByPatientAsync(int patientId);
    Task<bool> CreateAsync(CreateAllergyDto dto);
    Task<bool> UpdateAsync(UpdateAllergyDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> ChangeStateAsync(ChangeStateAllergyDto dto);
}
