using OnlineExamSystem.ViewModels;
using System.Linq.Expressions;

namespace OnlineExamSystem.Repository;

public interface IGenericRepository<T>:IDisposable
{
    IEnumerable<T> GetAll(
        Expression<Func<T,bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null,
        string includeProperties= "");

    T GetByID(object id);  
    Task<T> GetByIdAsync(object id);
    void Add (T entity);    
    Task<T> AddAsync (T entity);    
    void DeleteByID(object id);
    void Delete (T entityToDelete); 
    void Update (T entityToUpdate); 
    Task<T> UpdateAsync (T entityToUpdate);
    Task<T> DeleteAsync (T entityToDelete);
    Task AddAsync(GroupViewModel objGroup);
}
