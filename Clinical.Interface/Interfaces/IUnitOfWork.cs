using Clinical.Domain.Entities;

namespace Clinical.Interface.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Analysis> Analysis { get; }
        IGenericRepository<Exam> Exam { get; }
    }
}
