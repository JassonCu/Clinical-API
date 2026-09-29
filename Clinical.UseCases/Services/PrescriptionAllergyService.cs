using Clinical.Application.DTOS.Prescription.Response;
using Clinical.Interface.Interfaces;
using Clinical.Utils.Constants;
using Entity = Clinical.Domain.Entities;

namespace Clinical.UseCases.Services
{
    public class PrescriptionAllergyService : IPrescriptionAllergyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PrescriptionAllergyService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IReadOnlyList<AllergyConflictDto>> FindConflictsAsync(int patientId, IEnumerable<int> medicineIds)
        {
            var ids = medicineIds?.Where(id => id > 0).Distinct().ToList() ?? [];
            var conflicts = new List<AllergyConflictDto>();
            if (ids.Count == 0) return conflicts;

            var allergies = (await _unitOfWork.PatientAllergy.GetAllAsync(
                    StoreProcedures.uspAllergyByPatient, new { PatientId = patientId }))
                .Where(a => a.State == 1 && !string.IsNullOrWhiteSpace(a.AllergenName))
                .ToList();
            if (allergies.Count == 0) return conflicts;

            var medicines = (await _unitOfWork.Medicine.GetAllAsync(StoreProcedures.uspMedicineList))
                .Where(m => m.MedicineId.HasValue && ids.Contains(m.MedicineId.Value))
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

            return conflicts;
        }

        // Heuristic name match against the medicine's name/generic/brand/category.
        // Advisory — a curated drug-allergy database would be the v2.
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
