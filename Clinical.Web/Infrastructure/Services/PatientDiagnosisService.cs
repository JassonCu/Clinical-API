using Clinical.Web.Core.DTOs.PatientDiagnosis;
using Clinical.Web.Core.Interfaces;

namespace Clinical.Web.Infrastructure.Services;

public class PatientDiagnosisService : BaseApiService, IPatientDiagnosisService
{
    public PatientDiagnosisService(IHttpClientFactory factory, IHttpContextAccessor accessor, ILogger<PatientDiagnosisService> logger)
        : base(factory, accessor, logger) { }

    public Task<IEnumerable<DiagnosisDto>> GetAllAsync() =>
        GetAsync<IEnumerable<DiagnosisDto>>("/api/patientdiagnosis").ContinueWith(t => t.Result ?? []);

    public Task<DiagnosisDto?> GetByIdAsync(int id) => GetAsync<DiagnosisDto>($"/api/patientdiagnosis/{id}");

    public Task<IEnumerable<DiagnosisDto>> GetByPatientAsync(int patientId) =>
        GetAsync<IEnumerable<DiagnosisDto>>($"/api/patientdiagnosis/ByPatient/{patientId}").ContinueWith(t => t.Result ?? []);

    public Task<IEnumerable<DiagnosisDto>> GetByAppointmentAsync(int appointmentId) =>
        GetAsync<IEnumerable<DiagnosisDto>>($"/api/patientdiagnosis/ByAppointment/{appointmentId}").ContinueWith(t => t.Result ?? []);

    public async Task<bool> CreateAsync(CreateDiagnosisDto dto) { var (ok, _) = await PostAsync("/api/patientdiagnosis/Register", dto); return ok; }
    public async Task<bool> DeleteAsync(int id) { var (ok, _) = await DeleteAsync($"/api/patientdiagnosis/Remove/{id}"); return ok; }
    public async Task<bool> ChangeStateAsync(ChangeStateDiagnosisDto dto) { var (ok, _) = await PatchAsync("/api/patientdiagnosis/ChangeState", dto); return ok; }
}
