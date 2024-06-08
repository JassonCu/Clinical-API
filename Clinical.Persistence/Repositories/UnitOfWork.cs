using Clinical.Domain.Entities;
using Clinical.Interface.Interfaces;

namespace Clinical.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IGenericRepository<Analysis> Analysis { get; }

        public IGenericRepository<Exam> Exam { get; }

        public UnitOfWork(IGenericRepository<Analysis> analysis, IGenericRepository<Exam> exam)
        {
            Analysis = analysis;
            Exam = exam;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
