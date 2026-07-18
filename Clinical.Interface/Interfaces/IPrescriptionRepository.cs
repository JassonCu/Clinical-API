using Clinical.Application.DTOS.Prescription.Response;

namespace Clinical.Interface.Interfaces
{
    public interface IPrescriptionRepository
    {
        Task<IEnumerable<GetAllPrescriptionResponseDto>> GetAllPrescriptions(string storedProcedure);
        Task<GetPrescriptionByIdResponseDto?> GetPrescriptionById(string storedProcedure, object parameter);
        Task<IEnumerable<GetAllPrescriptionResponseDto>> GetPrescriptionsByPatient(string storedProcedure, object parameter);
        Task<IEnumerable<GetAllPrescriptionResponseDto>> GetPrescriptionsByDoctor(string storedProcedure, object parameter);
    }
}
