using Clinical.Application.DTOS.Prescription.Response;
using Clinical.Domain.Entities;

namespace Clinical.Interface.Interfaces
{
    public interface IPrescriptionRepository
    {
        Task<IEnumerable<GetAllPrescriptionResponseDto>> GetAllPrescriptions(string storedProcedure);
        Task<GetPrescriptionByIdResponseDto?> GetPrescriptionById(string storedProcedure, object parameter);
        Task<IEnumerable<GetAllPrescriptionResponseDto>> GetPrescriptionsByPatient(string storedProcedure, object parameter);
        Task<IEnumerable<GetAllPrescriptionResponseDto>> GetPrescriptionsByDoctor(string storedProcedure, object parameter);

        /// <summary>Inserts the prescription header and its detail lines in a single transaction; returns the new id.</summary>
        Task<int> CreateWithDetailsAsync(Prescription prescription, IEnumerable<PrescriptionDetail> details);
    }
}
