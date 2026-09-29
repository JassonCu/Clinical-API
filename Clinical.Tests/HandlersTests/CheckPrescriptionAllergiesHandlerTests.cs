using Clinical.Domain.Entities;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.UseCases.Prescription.Queries.CheckAllergiesQuery;
using Moq;

namespace Clinical.Test.HandlersTests
{
    public class CheckPrescriptionAllergiesHandlerTests
    {
        private static CheckPrescriptionAllergiesHandler Build(
            IEnumerable<PatientAllergy> allergies, IEnumerable<Medicine> medicines)
        {
            var allergyRepo = new Mock<IGenericRepository<PatientAllergy>>();
            allergyRepo.Setup(r => r.GetAllAsync(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(allergies);

            var medicineRepo = new Mock<IGenericRepository<Medicine>>();
            medicineRepo.Setup(r => r.GetAllAsync(It.IsAny<string>())).ReturnsAsync(medicines);

            var uow = new Mock<IUnitOfWork>();
            uow.Setup(u => u.PatientAllergy).Returns(allergyRepo.Object);
            uow.Setup(u => u.Medicine).Returns(medicineRepo.Object);
            return new CheckPrescriptionAllergiesHandler(uow.Object);
        }

        [Fact]
        public async Task Detects_Conflict_When_MedicineMatchesActiveAllergy()
        {
            var handler = Build(
                allergies: [new PatientAllergy { AllergenName = "Penicilina", Severity = "Alta", Reaction = "Anafilaxia", State = 1 }],
                medicines: [new Medicine { MedicineId = 1, Name = "Penival", GenericName = "Penicilina G Sódica" }]);

            var result = await handler.Handle(
                new CheckPrescriptionAllergiesQuery { PatientId = 5, MedicineIds = [1] }, CancellationToken.None);

            Assert.True(result.Data!.HasConflicts);
            var conflict = Assert.Single(result.Data.Conflicts);
            Assert.Equal(1, conflict.MedicineId);
            Assert.Equal("Penicilina", conflict.AllergenName);
            Assert.Equal("Alta", conflict.Severity);
        }

        [Fact]
        public async Task No_Conflict_When_NoMedicineMatches()
        {
            var handler = Build(
                allergies: [new PatientAllergy { AllergenName = "Maní", State = 1 }],
                medicines: [new Medicine { MedicineId = 1, Name = "Ibupirac", GenericName = "Ibuprofeno" }]);

            var result = await handler.Handle(
                new CheckPrescriptionAllergiesQuery { PatientId = 5, MedicineIds = [1] }, CancellationToken.None);

            Assert.False(result.Data!.HasConflicts);
            Assert.Empty(result.Data.Conflicts);
        }

        [Fact]
        public async Task Ignores_InactiveAllergy()
        {
            var handler = Build(
                allergies: [new PatientAllergy { AllergenName = "Penicilina", State = 0 }],
                medicines: [new Medicine { MedicineId = 1, GenericName = "Penicilina G Sódica" }]);

            var result = await handler.Handle(
                new CheckPrescriptionAllergiesQuery { PatientId = 5, MedicineIds = [1] }, CancellationToken.None);

            Assert.False(result.Data!.HasConflicts);
        }

        [Fact]
        public async Task No_MedicineIds_ReturnsNoConflicts()
        {
            var handler = Build(
                allergies: [new PatientAllergy { AllergenName = "Penicilina", State = 1 }],
                medicines: [new Medicine { MedicineId = 1, GenericName = "Penicilina G Sódica" }]);

            var result = await handler.Handle(
                new CheckPrescriptionAllergiesQuery { PatientId = 5, MedicineIds = [] }, CancellationToken.None);

            Assert.False(result.Data!.HasConflicts);
        }
    }
}
