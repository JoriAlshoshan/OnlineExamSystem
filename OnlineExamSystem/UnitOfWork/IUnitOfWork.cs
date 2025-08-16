using OnlineExamSystem.Repository;

namespace OnlineExamSystem.UnitOfWork;

public interface IUnitOfWork
{
    IGenericRepository<T> GenericRepository<T>() where T : class;
    void Save();

}
