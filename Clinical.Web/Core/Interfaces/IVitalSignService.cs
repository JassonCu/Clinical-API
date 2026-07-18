using Clinical.Web.Core.DTOs.VitalSign;

namespace Clinical.Web.Core.Interfaces;

public interface IVitalSignService
{
    Task<IEnumerable<VitalSignDto>> GetAllAsync();
    Task<IEnumerable<VitalSignDto>> GetByPatientAsync(int patientId);
    Task<bool> CreateAsync(CreateVitalSignDto dto);
    Task<bool> DeleteAsync(int id);
}
