using Clinical.Web.Core.DTOs.Appointment;
using Clinical.Web.Core.Interfaces;

namespace Clinical.Web.Infrastructure.Services;

public class AppointmentService : BaseApiService, IAppointmentService
{
    public AppointmentService(IHttpClientFactory factory, IHttpContextAccessor accessor, ILogger<AppointmentService> logger)
        : base(factory, accessor, logger) { }

    public Task<IEnumerable<AppointmentListDto>> GetAllAsync() =>
        GetAsync<IEnumerable<AppointmentListDto>>("/api/appointment").ContinueWith(t => t.Result ?? []);

    public Task<AppointmentDetailDto?> GetByIdAsync(int id) =>
        GetAsync<AppointmentDetailDto>($"/api/appointment/{id}");

    public Task<IEnumerable<AppointmentListDto>> GetByPatientAsync(int patientId) =>
        GetAsync<IEnumerable<AppointmentListDto>>($"/api/appointment/ByPatient/{patientId}").ContinueWith(t => t.Result ?? []);

    public Task<IEnumerable<AppointmentListDto>> GetByDoctorAsync(int doctorId) =>
        GetAsync<IEnumerable<AppointmentListDto>>($"/api/appointment/ByDoctor/{doctorId}").ContinueWith(t => t.Result ?? []);

    public async Task<bool> CreateAsync(CreateAppointmentDto dto) { var (ok, _) = await PostAsync("/api/appointment/Register", dto); return ok; }
    public async Task<bool> UpdateAsync(UpdateAppointmentDto dto) { var (ok, _) = await PutAsync("/api/appointment/Edit", dto); return ok; }
    public async Task<bool> DeleteAsync(int id) { var (ok, _) = await DeleteAsync($"/api/appointment/Remove/{id}"); return ok; }
    public async Task<bool> ChangeStateAsync(ChangeStateAppointmentDto dto) { var (ok, _) = await PatchAsync("/api/appointment/ChangeState", dto); return ok; }
}
