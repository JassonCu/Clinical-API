using Clinical.Application.DTOS.Appointment.Response;

namespace Clinical.Interface.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<GetAllAppointmentResponseDto>> GetAllAppointments(string storedProcedure);
        Task<GetAppointmentByIdResponseDto> GetAppointmentById(string storedProcedure, object parameter);
        Task<IEnumerable<GetAllAppointmentResponseDto>> GetAppointmentsByPatient(string storedProcedure, object parameter);
        Task<IEnumerable<GetAllAppointmentResponseDto>> GetAppointmentsByDoctor(string storedProcedure, object parameter);
    }
}
