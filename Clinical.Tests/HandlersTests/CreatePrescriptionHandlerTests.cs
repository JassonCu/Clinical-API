using Clinical.Application.DTOS.Prescription.Response;
using Clinical.Domain.Entities;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Exceptions;
using Clinical.UseCases.UseCases.Prescription.Commands.CreateCommand;
using Moq;

namespace Clinical.Test.HandlersTests
{
    public class CreatePrescriptionHandlerTests
    {
        private static CreatePrescriptionCommand Command(bool acknowledge) => new()
        {
            PatientId = 5,
            DoctorId = 2,
            Details = [new CreatePrescriptionDetailItem { MedicineId = 1, Quantity = 1 }],
            AcknowledgeAllergyWarning = acknowledge
        };

        private static Mock<IPrescriptionAllergyService> AllergyServiceReturning(params AllergyConflictDto[] conflicts)
        {
            var service = new Mock<IPrescriptionAllergyService>();
            service.Setup(s => s.FindConflictsAsync(It.IsAny<int>(), It.IsAny<IEnumerable<int>>()))
                   .ReturnsAsync(conflicts.ToList());
            return service;
        }

        [Fact]
        public async Task Conflict_NotAcknowledged_ThrowsConflict_AndDoesNotPersist()
        {
            var allergy = AllergyServiceReturning(new AllergyConflictDto { MedicineId = 1, MedicineName = "Penival", AllergenName = "Penicilina" });
            var repo = new Mock<IPrescriptionRepository>();

            var handler = new CreatePrescriptionHandler(repo.Object, allergy.Object);

            await Assert.ThrowsAsync<ConflictException>(() =>
                handler.Handle(Command(acknowledge: false), CancellationToken.None));

            repo.Verify(r => r.CreateWithDetailsAsync(It.IsAny<Prescription>(), It.IsAny<IEnumerable<PrescriptionDetail>>()), Times.Never);
        }

        [Fact]
        public async Task Conflict_Acknowledged_SkipsCheck_AndPersists()
        {
            var allergy = AllergyServiceReturning(new AllergyConflictDto { MedicineId = 1, AllergenName = "Penicilina" });
            var repo = new Mock<IPrescriptionRepository>();
            repo.Setup(r => r.CreateWithDetailsAsync(It.IsAny<Prescription>(), It.IsAny<IEnumerable<PrescriptionDetail>>())).ReturnsAsync(10);

            var handler = new CreatePrescriptionHandler(repo.Object, allergy.Object);
            var result = await handler.Handle(Command(acknowledge: true), CancellationToken.None);

            Assert.True(result.IsSuccess);
            repo.Verify(r => r.CreateWithDetailsAsync(It.IsAny<Prescription>(), It.IsAny<IEnumerable<PrescriptionDetail>>()), Times.Once);
            // Acknowledged → the allergy check is skipped entirely.
            allergy.Verify(s => s.FindConflictsAsync(It.IsAny<int>(), It.IsAny<IEnumerable<int>>()), Times.Never);
        }

        [Fact]
        public async Task NoConflict_Persists()
        {
            var allergy = AllergyServiceReturning();
            var repo = new Mock<IPrescriptionRepository>();
            repo.Setup(r => r.CreateWithDetailsAsync(It.IsAny<Prescription>(), It.IsAny<IEnumerable<PrescriptionDetail>>())).ReturnsAsync(11);

            var handler = new CreatePrescriptionHandler(repo.Object, allergy.Object);
            var result = await handler.Handle(Command(acknowledge: false), CancellationToken.None);

            Assert.True(result.IsSuccess);
            repo.Verify(r => r.CreateWithDetailsAsync(It.IsAny<Prescription>(), It.IsAny<IEnumerable<PrescriptionDetail>>()), Times.Once);
        }
    }
}
