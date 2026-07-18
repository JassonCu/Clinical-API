using System.Data;
using Clinical.Application.DTOS.Prescription.Response;
using Clinical.Interface.Interfaces;
using Clinical.Persistence.Context;
using Dapper;

namespace Clinical.Persistence.Repositories
{
    public class PrescriptionRepository : IPrescriptionRepository
    {
        private readonly ApplicationDBContext _context;

        public PrescriptionRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GetAllPrescriptionResponseDto>> GetAllPrescriptions(string storedProcedure)
        {
            using var connection = _context.CreateConnection;
            return await connection.QueryAsync<GetAllPrescriptionResponseDto>(
                storedProcedure, commandType: CommandType.StoredProcedure);
        }

        public async Task<GetPrescriptionByIdResponseDto?> GetPrescriptionById(string storedProcedure, object parameter)
        {
            using var connection = _context.CreateConnection;
            var objParam = new DynamicParameters(parameter);
            var prescriptionDict = new Dictionary<int, GetPrescriptionByIdResponseDto>();

            await connection.QueryAsync<GetPrescriptionByIdResponseDto, PrescriptionDetailItemDto, GetPrescriptionByIdResponseDto>(
                storedProcedure,
                (prescription, detail) =>
                {
                    if (!prescriptionDict.TryGetValue(prescription.PrescriptionId, out var existing))
                    {
                        existing = prescription;
                        existing.Details = new List<PrescriptionDetailItemDto>();
                        prescriptionDict[prescription.PrescriptionId] = existing;
                    }
                    if (detail is not null)
                        ((List<PrescriptionDetailItemDto>)existing.Details!).Add(detail);
                    return existing;
                },
                param: objParam,
                commandType: CommandType.StoredProcedure,
                splitOn: "PrescriptionDetailId");

            return prescriptionDict.Values.FirstOrDefault();
        }

        public async Task<IEnumerable<GetAllPrescriptionResponseDto>> GetPrescriptionsByPatient(string storedProcedure, object parameter)
        {
            using var connection = _context.CreateConnection;
            var objParam = new DynamicParameters(parameter);
            return await connection.QueryAsync<GetAllPrescriptionResponseDto>(
                storedProcedure, param: objParam, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<GetAllPrescriptionResponseDto>> GetPrescriptionsByDoctor(string storedProcedure, object parameter)
        {
            using var connection = _context.CreateConnection;
            var objParam = new DynamicParameters(parameter);
            return await connection.QueryAsync<GetAllPrescriptionResponseDto>(
                storedProcedure, param: objParam, commandType: CommandType.StoredProcedure);
        }
    }
}
