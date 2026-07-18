using Clinical.Web.Core.DTOs.PatientDiagnosis;

namespace Clinical.Web.Core.Interfaces;

public interface IPatientDiagnosisService
{
    Task<IEnumerable<DiagnosisDto>> GetAllAsync();
    Task<DiagnosisDto?> GetByIdAsync(int id);
    Task<IEnumerable<DiagnosisDto>> GetByPatientAsync(int patientId);
    Task<IEnumerable<DiagnosisDto>> GetByAppointmentAsync(int appointmentId);
    Task<bool> CreateAsync(CreateDiagnosisDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> ChangeStateAsync(ChangeStateDiagnosisDto dto);
}
