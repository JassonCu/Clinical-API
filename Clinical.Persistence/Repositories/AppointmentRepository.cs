using System.Data;
using Clinical.Application.DTOS.Appointment.Response;
using Clinical.Interface.Interfaces;
using Clinical.Persistence.Context;
using Dapper;

namespace Clinical.Persistence.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ApplicationDBContext _context;

        public AppointmentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GetAllAppointmentResponseDto>> GetAllAppointments(string storedProcedure)
        {
            using var connection = _context.CreateConnection;
            return await connection.QueryAsync<GetAllAppointmentResponseDto>(storedProcedure, commandType: CommandType.StoredProcedure);
        }

        public async Task<GetAppointmentByIdResponseDto> GetAppointmentById(string storedProcedure, object parameter)
        {
            using var connection = _context.CreateConnection;
            var objParam = new DynamicParameters(parameter);
            return await connection.QuerySingleOrDefaultAsync<GetAppointmentByIdResponseDto>(storedProcedure, param: objParam, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<GetAllAppointmentResponseDto>> GetAppointmentsByPatient(string storedProcedure, object parameter)
        {
            using var connection = _context.CreateConnection;
            var objParam = new DynamicParameters(parameter);
            return await connection.QueryAsync<GetAllAppointmentResponseDto>(storedProcedure, param: objParam, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<GetAllAppointmentResponseDto>> GetAppointmentsByDoctor(string storedProcedure, object parameter)
        {
            using var connection = _context.CreateConnection;
            var objParam = new DynamicParameters(parameter);
            return await connection.QueryAsync<GetAllAppointmentResponseDto>(storedProcedure, param: objParam, commandType: CommandType.StoredProcedure);
        }
    }
}
