using Clinical.Domain.Entities;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Services;
using Moq;

namespace Clinical.Test.HandlersTests
{
    public class PrescriptionAllergyServiceTests
    {
        private static PrescriptionAllergyService Build(
            IEnumerable<PatientAllergy> allergies, IEnumerable<Medicine> medicines)
        {
            var allergyRepo = new Mock<IGenericRepository<PatientAllergy>>();
            allergyRepo.Setup(r => r.GetAllAsync(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(allergies);

            var medicineRepo = new Mock<IGenericRepository<Medicine>>();
            medicineRepo.Setup(r => r.GetAllAsync(It.IsAny<string>())).ReturnsAsync(medicines);

            var uow = new Mock<IUnitOfWork>();
            uow.Setup(u => u.PatientAllergy).Returns(allergyRepo.Object);
            uow.Setup(u => u.Medicine).Returns(medicineRepo.Object);
            return new PrescriptionAllergyService(uow.Object);
        }

        [Fact]
        public async Task Detects_Conflict_When_MedicineMatchesActiveAllergy()
        {
            var service = Build(
                allergies: [new PatientAllergy { AllergenName = "Penicilina", Severity = "Alta", Reaction = "Anafilaxia", State = 1 }],
                medicines: [new Medicine { MedicineId = 1, Name = "Penival", GenericName = "Penicilina G Sódica" }]);

            var conflicts = await service.FindConflictsAsync(5, [1]);

            var conflict = Assert.Single(conflicts);
            Assert.Equal(1, conflict.MedicineId);
            Assert.Equal("Penicilina", conflict.AllergenName);
            Assert.Equal("Alta", conflict.Severity);
        }

        [Fact]
        public async Task No_Conflict_When_NoMedicineMatches()
        {
            var service = Build(
                allergies: [new PatientAllergy { AllergenName = "Maní", State = 1 }],
                medicines: [new Medicine { MedicineId = 1, Name = "Ibupirac", GenericName = "Ibuprofeno" }]);

            Assert.Empty(await service.FindConflictsAsync(5, [1]));
        }

        [Fact]
        public async Task Ignores_InactiveAllergy()
        {
            var service = Build(
                allergies: [new PatientAllergy { AllergenName = "Penicilina", State = 0 }],
                medicines: [new Medicine { MedicineId = 1, GenericName = "Penicilina G Sódica" }]);

            Assert.Empty(await service.FindConflictsAsync(5, [1]));
        }

        [Fact]
        public async Task No_MedicineIds_ReturnsNoConflicts()
        {
            var service = Build(
                allergies: [new PatientAllergy { AllergenName = "Penicilina", State = 1 }],
                medicines: [new Medicine { MedicineId = 1, GenericName = "Penicilina G Sódica" }]);

            Assert.Empty(await service.FindConflictsAsync(5, []));
        }
    }
}
