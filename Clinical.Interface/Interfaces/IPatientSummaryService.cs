using Clinical.Application.DTOS.Patient.Response;

namespace Clinical.Interface.Interfaces
{
    /// <summary>
    /// Builds a consolidated 360° clinical summary for a patient by aggregating
    /// allergies, diagnoses, prescriptions, appointments, vital signs and medical history.
    /// </summary>
    public interface IPatientSummaryService
    {
        /// <summary>Returns the consolidated summary, or null if the patient does not exist.</summary>
        Task<PatientSummaryDto?> BuildAsync(int patientId);
    }
}
