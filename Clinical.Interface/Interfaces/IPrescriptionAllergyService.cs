using Clinical.Application.DTOS.Prescription.Response;

namespace Clinical.Interface.Interfaces
{
    /// <summary>
    /// Finds conflicts between a set of medicines and a patient's active allergies.
    /// Shared by the advisory check endpoint and the server-side enforcement at prescription creation.
    /// </summary>
    public interface IPrescriptionAllergyService
    {
        Task<IReadOnlyList<AllergyConflictDto>> FindConflictsAsync(int patientId, IEnumerable<int> medicineIds);
    }
}
