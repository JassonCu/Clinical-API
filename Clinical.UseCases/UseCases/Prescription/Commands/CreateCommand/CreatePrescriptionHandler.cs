using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.UseCases.Commons.Exceptions;
using Clinical.Utils.Constants;
using MediatR;
using Entity = Clinical.Domain.Entities;

namespace Clinical.UseCases.UseCases.Prescription.Commands.CreateCommand
{
    public class CreatePrescriptionHandler : IRequestHandler<CreatePrescriptionCommand, BaseResponse<bool>>
    {
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly IPrescriptionAllergyService _allergyService;

        public CreatePrescriptionHandler(
            IPrescriptionRepository prescriptionRepository, IPrescriptionAllergyService allergyService)
        {
            _prescriptionRepository = prescriptionRepository;
            _allergyService = allergyService;
        }

        public async Task<BaseResponse<bool>> Handle(CreatePrescriptionCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();

            // Enforcement: refuse to persist a prescription that conflicts with the patient's registered
            // allergies, unless the prescriber explicitly acknowledged the warning (safety backstop that
            // holds even if the UI check is bypassed).
            if (!request.AcknowledgeAllergyWarning && request.PatientId is int patientId)
            {
                var conflicts = await _allergyService.FindConflictsAsync(
                    patientId, request.Details.Select(d => d.MedicineId));

                if (conflicts.Count > 0)
                {
                    var detail = string.Join(", ", conflicts.Select(c => $"{c.MedicineName} (alergia: {c.AllergenName})"));
                    throw new ConflictException(
                        $"La receta incluye medicamentos con alergias registradas del paciente: {detail}. Confirme para continuar.");
                }
            }

            var prescription = new Entity.Prescription
            {
                PatientId = request.PatientId,
                DoctorId = request.DoctorId,
                AppointmentId = request.AppointmentId,
                PrescriptionDate = request.PrescriptionDate ?? DateTime.UtcNow,
                ValidUntil = request.ValidUntil,
                Notes = request.Notes,
                State = 1
            };

            var details = request.Details.Select(d => new Entity.PrescriptionDetail
            {
                MedicineId = d.MedicineId,
                Quantity = d.Quantity,
                Dosage = d.Dosage,
                Frequency = d.Frequency,
                Duration = d.Duration,
                Instructions = d.Instructions
            });

            var newId = await _prescriptionRepository.CreateWithDetailsAsync(prescription, details);

            response.IsSuccess = newId > 0;
            response.Data = response.IsSuccess;
            response.Message = response.IsSuccess ? GlobalMessage.MESSAGE_SAVE : GlobalMessage.MESSAGE_FAILED;
            return response;
        }
    }
}
