using Clinical.Web.Core.DTOs.MedicalHistory;

namespace Clinical.Web.Core.Interfaces;

public interface IMedicalHistoryService
{
    Task<MedicalHistoryDto?> GetByIdAsync(int id);
    Task<IEnumerable<MedicalHistoryDto>> GetByPatientAsync(int patientId);
    Task<bool> CreateAsync(CreateMedicalHistoryDto dto);
    Task<bool> UpdateAsync(UpdateMedicalHistoryDto dto);
    Task<bool> DeleteAsync(int id);
}
