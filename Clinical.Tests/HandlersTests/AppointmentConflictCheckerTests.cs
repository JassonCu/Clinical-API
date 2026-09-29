using Clinical.Domain.Entities;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Services;
using Microsoft.Extensions.Configuration;
using Moq;
// Microsoft.Extensions.Configuration.Abstractions is available transitively via Clinical.UseCases.

namespace Clinical.Test.HandlersTests
{
    public class AppointmentConflictCheckerTests
    {
        private static readonly DateTime Base = new(2026, 10, 1, 10, 0, 0);

        private static AppointmentConflictChecker Build(int slotMinutes, params Appointment[] doctorAgenda)
        {
            var repo = new Mock<IGenericRepository<Appointment>>();
            repo.Setup(r => r.GetAllAsync(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(doctorAgenda);

            var uow = new Mock<IUnitOfWork>();
            uow.Setup(u => u.Appointment).Returns(repo.Object);

            var config = new Mock<IConfiguration>();
            config.Setup(c => c["Appointment:SlotMinutes"]).Returns(slotMinutes.ToString());

            return new AppointmentConflictChecker(uow.Object, config.Object);
        }

        [Fact]
        public async Task Detects_Conflict_Within_Slot_Window()
        {
            var checker = Build(30,
                new Appointment { AppointmentId = 7, AppointmentDate = Base.AddMinutes(15), State = 1 });

            var conflict = await checker.FindConflictAsync(doctorId: 2, when: Base);

            Assert.NotNull(conflict);
            Assert.Equal(7, conflict!.AppointmentId);
        }

        [Fact]
        public async Task No_Conflict_When_Outside_Slot_Window()
        {
            var checker = Build(30,
                new Appointment { AppointmentId = 7, AppointmentDate = Base.AddMinutes(30), State = 1 });

            Assert.Null(await checker.FindConflictAsync(doctorId: 2, when: Base));
        }

        [Fact]
        public async Task Excludes_The_Same_Appointment_When_Rescheduling()
        {
            var checker = Build(30,
                new Appointment { AppointmentId = 7, AppointmentDate = Base, State = 1 });

            Assert.Null(await checker.FindConflictAsync(doctorId: 2, when: Base, excludeAppointmentId: 7));
        }

        [Fact]
        public async Task Ignores_Cancelled_Appointments()
        {
            var checker = Build(30,
                new Appointment { AppointmentId = 7, AppointmentDate = Base, State = 0 });

            Assert.Null(await checker.FindConflictAsync(doctorId: 2, when: Base));
        }
    }
}
