using Clinical.Domain.Entities;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Services;
using Moq;

namespace Clinical.Test.HandlersTests
{
    public class PatientSummaryServiceTests
    {
        private static readonly DateTime Now = DateTime.Now;

        private static Mock<IGenericRepository<T>> Repo<T>(IEnumerable<T> all) where T : class
        {
            var repo = new Mock<IGenericRepository<T>>();
            repo.Setup(r => r.GetAllAsync(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(all);
            return repo;
        }

        private static PatientSummaryService Build(
            Patient? patient,
            IEnumerable<PatientAllergy>? allergies = null,
            IEnumerable<PatientDiagnosis>? diagnoses = null,
            IEnumerable<Prescription>? prescriptions = null,
            IEnumerable<Appointment>? appointments = null,
            IEnumerable<VitalSign>? vitals = null,
            IEnumerable<MedicalHistory>? histories = null)
        {
            var patientRepo = new Mock<IGenericRepository<Patient>>();
            patientRepo.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(patient!);

            var uow = new Mock<IUnitOfWork>();
            uow.Setup(u => u.Patient).Returns(patientRepo.Object);
            uow.Setup(u => u.PatientAllergy).Returns(Repo(allergies ?? []).Object);
            uow.Setup(u => u.PatientDiagnosis).Returns(Repo(diagnoses ?? []).Object);
            uow.Setup(u => u.Prescription).Returns(Repo(prescriptions ?? []).Object);
            uow.Setup(u => u.Appointment).Returns(Repo(appointments ?? []).Object);
            uow.Setup(u => u.VitalSign).Returns(Repo(vitals ?? []).Object);
            uow.Setup(u => u.MedicalHistory).Returns(Repo(histories ?? []).Object);
            return new PatientSummaryService(uow.Object);
        }

        [Fact]
        public async Task Returns_Null_When_Patient_Not_Found()
        {
            var service = Build(patient: null);
            Assert.Null(await service.BuildAsync(99));
        }

        [Fact]
        public async Task Builds_Header_With_Computed_Age()
        {
            var dob = Now.AddYears(-30).AddDays(-5);
            var service = Build(new Patient { PatientId = 5, FirstName = "Ana", LastName = "Pérez", BirthDate = dob, MobilePhone = "555", Phone = "111" });

            var summary = await service.BuildAsync(5);

            Assert.NotNull(summary);
            Assert.Equal("Ana Pérez", summary!.Patient.FullName);
            Assert.Equal(30, summary.Patient.Age);
            Assert.Equal("555", summary.Patient.Phone); // mobile preferred over landline
        }

        [Fact]
        public async Task Filters_Inactive_Allergies_And_Counts_Active()
        {
            var service = Build(
                new Patient { PatientId = 5 },
                allergies:
                [
                    new PatientAllergy { AllergenName = "Penicilina", Severity = "Alta", State = 1 },
                    new PatientAllergy { AllergenName = "Ibuprofeno", State = 0 },            // inactive
                    new PatientAllergy { AllergenName = "", State = 1 }                        // empty name
                ]);

            var summary = await service.BuildAsync(5);

            Assert.Single(summary!.ActiveAllergies);
            Assert.Equal("Penicilina", summary.ActiveAllergies[0].AllergenName);
            Assert.Equal(1, summary.Counts.ActiveAllergies);
        }

        [Fact]
        public async Task Splits_Upcoming_And_Last_Appointment()
        {
            var service = Build(
                new Patient { PatientId = 5 },
                appointments:
                [
                    new Appointment { AppointmentId = 1, AppointmentDate = Now.AddDays(-10), State = 1 },
                    new Appointment { AppointmentId = 2, AppointmentDate = Now.AddDays(-2), State = 1 },  // most recent past
                    new Appointment { AppointmentId = 3, AppointmentDate = Now.AddDays(3), State = 1 },   // upcoming
                    new Appointment { AppointmentId = 4, AppointmentDate = Now.AddDays(5), State = 0 }    // cancelled → excluded
                ]);

            var summary = await service.BuildAsync(5);

            Assert.Single(summary!.UpcomingAppointments);
            Assert.Equal(3, summary.UpcomingAppointments[0].AppointmentId);
            Assert.Equal(2, summary.LastAppointment!.AppointmentId);
            Assert.Equal(4, summary.Counts.TotalAppointments);
        }

        [Fact]
        public async Task Marks_Prescription_Active_By_ValidUntil()
        {
            var service = Build(
                new Patient { PatientId = 5 },
                prescriptions:
                [
                    new Prescription { PrescriptionId = 1, PrescriptionDate = Now.AddDays(-1), ValidUntil = Now.AddDays(10), State = 1 },
                    new Prescription { PrescriptionId = 2, PrescriptionDate = Now.AddDays(-40), ValidUntil = Now.AddDays(-5), State = 1 }
                ]);

            var summary = await service.BuildAsync(5);

            // Ordered by date desc → first is #1 (vigente), second is #2 (vencida).
            Assert.True(summary!.RecentPrescriptions[0].IsActive);
            Assert.False(summary.RecentPrescriptions[1].IsActive);
        }

        [Fact]
        public async Task Picks_Latest_VitalSign()
        {
            var service = Build(
                new Patient { PatientId = 5 },
                vitals:
                [
                    new VitalSign { HeartRate = 70, MeasuredAt = Now.AddDays(-5) },
                    new VitalSign { HeartRate = 88, MeasuredAt = Now.AddDays(-1) }
                ]);

            var summary = await service.BuildAsync(5);

            Assert.Equal(88, summary!.LatestVitalSign!.HeartRate);
        }
    }
}
