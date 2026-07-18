using Clinical.Web.Core.DTOs.Appointment;

namespace Clinical.Web.Core.Interfaces;

public interface IAppointmentService
{
    Task<IEnumerable<AppointmentListDto>> GetAllAsync();
    Task<AppointmentDetailDto?> GetByIdAsync(int id);
    Task<IEnumerable<AppointmentListDto>> GetByPatientAsync(int patientId);
    Task<IEnumerable<AppointmentListDto>> GetByDoctorAsync(int doctorId);
    Task<bool> CreateAsync(CreateAppointmentDto dto);
    Task<bool> UpdateAsync(UpdateAppointmentDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> ChangeStateAsync(ChangeStateAppointmentDto dto);
}
