using Clinical.Web.Core.DTOs.Prescription;

namespace Clinical.Web.Core.Interfaces;

public interface IPrescriptionService
{
    Task<IEnumerable<PrescriptionListDto>> GetAllAsync();
    Task<PrescriptionDetailDto?> GetByIdAsync(int id);
    Task<IEnumerable<PrescriptionListDto>> GetByPatientAsync(int patientId);
    Task<IEnumerable<PrescriptionListDto>> GetByDoctorAsync(int doctorId);
    Task<bool> CreateAsync(CreatePrescriptionDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> ChangeStateAsync(ChangeStatePrescriptionDto dto);
}
