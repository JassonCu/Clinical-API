using Clinical.Domain.Entities;
using Clinical.Interface.Interfaces;

namespace Clinical.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IGenericRepository<Analysis> Analysis { get; }

        public UnitOfWork(IGenericRepository<Analysis> analysis)
        {
            Analysis = analysis;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
