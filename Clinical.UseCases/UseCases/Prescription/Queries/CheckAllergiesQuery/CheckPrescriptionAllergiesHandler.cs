using Clinical.Application.DTOS.Prescription.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;
using Entity = Clinical.Domain.Entities;

namespace Clinical.UseCases.UseCases.Prescription.Queries.CheckAllergiesQuery
{
    public class CheckPrescriptionAllergiesHandler
        : IRequestHandler<CheckPrescriptionAllergiesQuery, BaseResponse<AllergyCheckResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CheckPrescriptionAllergiesHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<BaseResponse<AllergyCheckResponseDto>> Handle(
            CheckPrescriptionAllergiesQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<AllergyCheckResponseDto>();
            var conflicts = new List<AllergyConflictDto>();

            var allergies = (await _unitOfWork.PatientAllergy.GetAllAsync(
                    StoreProcedures.uspAllergyByPatient, new { request.PatientId }))
                .Where(a => a.State == 1 && !string.IsNullOrWhiteSpace(a.AllergenName))
                .ToList();

            if (allergies.Count > 0 && request.MedicineIds.Count > 0)
            {
                var medicines = (await _unitOfWork.Medicine.GetAllAsync(StoreProcedures.uspMedicineList))
                    .Where(m => m.MedicineId.HasValue && request.MedicineIds.Contains(m.MedicineId.Value))
                    .ToList();

                foreach (var medicine in medicines)
                {
                    var allergy = allergies.FirstOrDefault(a => IsMatch(a.AllergenName!, medicine));
                    if (allergy is not null)
                    {
                        conflicts.Add(new AllergyConflictDto
                        {
                            MedicineId = medicine.MedicineId!.Value,
                            MedicineName = medicine.Name ?? string.Empty,
                            AllergenName = allergy.AllergenName!,
                            Severity = allergy.Severity,
                            Reaction = allergy.Reaction
                        });
                    }
                }
            }

            response.IsSuccess = true;
            response.Message = GlobalMessage.MESSAGE_QUERY;
            response.Data = new AllergyCheckResponseDto
            {
                HasConflicts = conflicts.Count > 0,
                Conflicts = conflicts
            };
            return response;
        }

        // Heuristic name match against the medicine's name/generic/brand/category.
        // Advisory only — surfaces a warning; a curated drug-allergy database would be the v2.
        private static bool IsMatch(string allergen, Entity.Medicine medicine)
        {
            var normalized = allergen.Trim().ToLowerInvariant();
            if (normalized.Length < 4) return false;

            return FieldMatches(medicine.Name, normalized)
                || FieldMatches(medicine.GenericName, normalized)
                || FieldMatches(medicine.Brand, normalized)
                || FieldMatches(medicine.Category, normalized);
        }

        private static bool FieldMatches(string? field, string allergen)
        {
            if (string.IsNullOrWhiteSpace(field)) return false;
            var value = field.Trim().ToLowerInvariant();
            return value.Contains(allergen) || allergen.Contains(value);
        }
    }
}
