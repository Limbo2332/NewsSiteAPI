using NewsSite.DAL.Entities.Abstract;

namespace NewsSite.DAL.Repositories.Base;

public interface IGenericRepository<T> where T : BaseEntity
{
    IQueryable<T> GetAll();

    Task<T?> GetByIdAsync(Guid id);

    Task AddAsync(T entity);

    Task UpdateAsync(T entity);

    Task DeleteAsync(Guid id);
}