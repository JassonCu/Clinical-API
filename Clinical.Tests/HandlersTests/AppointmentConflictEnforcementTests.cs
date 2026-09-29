using AutoMapper;
using Clinical.Domain.Entities;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Exceptions;
using Clinical.UseCases.UseCases.Appointment.Commands.CreateCommand;
using Clinical.UseCases.UseCases.Appointment.Commands.UpdateCommand;
using Moq;

namespace Clinical.Test.HandlersTests
{
    public class AppointmentConflictEnforcementTests
    {
        private static Mock<IAppointmentConflictChecker> CheckerReturning(Appointment? conflict)
        {
            var checker = new Mock<IAppointmentConflictChecker>();
            checker.Setup(c => c.FindConflictAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<int?>()))
                   .ReturnsAsync(conflict);
            return checker;
        }

        // ---- Create ----

        [Fact]
        public async Task Create_Conflict_ThrowsConflict_AndDoesNotPersist()
        {
            var checker = CheckerReturning(new Appointment { AppointmentId = 9, AppointmentDate = DateTime.Now });
            var repo = new Mock<IGenericRepository<Appointment>>();
            var uow = new Mock<IUnitOfWork>();
            uow.Setup(u => u.Appointment).Returns(repo.Object);

            var handler = new CreateAppointmentHandler(uow.Object, Mock.Of<IMapper>(), checker.Object);

            await Assert.ThrowsAsync<ConflictException>(() =>
                handler.Handle(new CreateAppointmentCommand { DoctorId = 2, AppointmentDate = DateTime.Now.AddDays(1) }, CancellationToken.None));

            repo.Verify(r => r.ExecAsync(It.IsAny<string>(), It.IsAny<object>()), Times.Never);
        }

        [Fact]
        public async Task Create_NoConflict_Persists()
        {
            var checker = CheckerReturning(null);
            var repo = new Mock<IGenericRepository<Appointment>>();
            repo.Setup(r => r.ExecAsync(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(true);
            var uow = new Mock<IUnitOfWork>();
            uow.Setup(u => u.Appointment).Returns(repo.Object);

            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<Appointment>(It.IsAny<object>())).Returns(new Appointment());

            var handler = new CreateAppointmentHandler(uow.Object, mapper.Object, checker.Object);
            var result = await handler.Handle(
                new CreateAppointmentCommand { DoctorId = 2, AppointmentDate = DateTime.Now.AddDays(1) }, CancellationToken.None);

            Assert.True(result.IsSuccess);
            repo.Verify(r => r.ExecAsync(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
        }

        // ---- Update (reschedule) ----

        [Fact]
        public async Task Update_Reschedule_Conflict_ThrowsConflict_AndDoesNotPersist()
        {
            var checker = CheckerReturning(new Appointment { AppointmentId = 9, AppointmentDate = DateTime.Now });
            var repo = new Mock<IGenericRepository<Appointment>>();
            repo.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(new Appointment { AppointmentId = 5, DoctorId = 2 });
            var uow = new Mock<IUnitOfWork>();
            uow.Setup(u => u.Appointment).Returns(repo.Object);

            var handler = new UpdateAppointmentHandler(uow.Object, Mock.Of<IMapper>(), checker.Object);

            await Assert.ThrowsAsync<ConflictException>(() =>
                handler.Handle(new UpdateAppointmentCommand { AppointmentId = 5, AppointmentDate = DateTime.Now.AddDays(1) }, CancellationToken.None));

            checker.Verify(c => c.FindConflictAsync(2, It.IsAny<DateTime>(), 5), Times.Once);
            repo.Verify(r => r.ExecAsync(It.IsAny<string>(), It.IsAny<object>()), Times.Never);
        }

        [Fact]
        public async Task Update_WithoutRescheduling_SkipsConflictCheck()
        {
            var checker = CheckerReturning(new Appointment { AppointmentId = 9, AppointmentDate = DateTime.Now });
            var repo = new Mock<IGenericRepository<Appointment>>();
            repo.Setup(r => r.ExecAsync(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(true);
            var uow = new Mock<IUnitOfWork>();
            uow.Setup(u => u.Appointment).Returns(repo.Object);

            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<Appointment>(It.IsAny<object>())).Returns(new Appointment());

            var handler = new UpdateAppointmentHandler(uow.Object, mapper.Object, checker.Object);
            // AppointmentDate == null → not rescheduling → no conflict check, no fetch.
            var result = await handler.Handle(
                new UpdateAppointmentCommand { AppointmentId = 5, Diagnosis = "Faringitis" }, CancellationToken.None);

            Assert.True(result.IsSuccess);
            checker.Verify(c => c.FindConflictAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<int?>()), Times.Never);
            repo.Verify(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<object>()), Times.Never);
        }
    }
}
