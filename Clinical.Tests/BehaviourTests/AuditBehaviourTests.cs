using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Behaviours;
using MediatR;
using Moq;

namespace Clinical.Test.BehaviourTests
{
    public class AuditBehaviourTests
    {
        public class SampleCommand { public int PatientId { get; set; } }
        public class SampleQuery { public int PatientId { get; set; } }

        private static (AuditBehaviour<TReq, string> behaviour, Mock<IAuditLogger> audit) Build<TReq>() where TReq : notnull
        {
            var audit = new Mock<IAuditLogger>();
            var user = new Mock<ICurrentUserService>();
            user.SetupGet(u => u.UserId).Returns(7);
            user.SetupGet(u => u.UserName).Returns("admin");
            return (new AuditBehaviour<TReq, string>(audit.Object, user.Object), audit);
        }

        [Fact]
        public async Task Command_Success_RecordsAuditEntry_WithUserAndEntityId()
        {
            var (behaviour, audit) = Build<SampleCommand>();
            RequestHandlerDelegate<string> next = () => Task.FromResult("ok");

            var result = await behaviour.Handle(new SampleCommand { PatientId = 42 }, next, CancellationToken.None);

            Assert.Equal("ok", result);
            audit.Verify(a => a.RecordAsync(It.Is<AuditEntry>(e =>
                e.Action == "SampleCommand" && e.Outcome == "Success" &&
                e.EntityId == "42" && e.UserId == 7 && e.UserName == "admin")), Times.Once);
        }

        [Fact]
        public async Task Command_Failure_RecordsFailed_AndRethrows()
        {
            var (behaviour, audit) = Build<SampleCommand>();
            RequestHandlerDelegate<string> next = () => throw new InvalidOperationException("boom");

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                behaviour.Handle(new SampleCommand { PatientId = 1 }, next, CancellationToken.None));

            audit.Verify(a => a.RecordAsync(It.Is<AuditEntry>(e => e.Outcome == "Failed")), Times.Once);
        }

        [Fact]
        public async Task Query_IsNotAudited()
        {
            var (behaviour, audit) = Build<SampleQuery>();
            RequestHandlerDelegate<string> next = () => Task.FromResult("data");

            var result = await behaviour.Handle(new SampleQuery { PatientId = 1 }, next, CancellationToken.None);

            Assert.Equal("data", result);
            audit.Verify(a => a.RecordAsync(It.IsAny<AuditEntry>()), Times.Never);
        }
    }
}
