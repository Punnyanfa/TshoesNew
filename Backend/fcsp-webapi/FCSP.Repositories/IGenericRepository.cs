using FCSP.Models.Entities;

namespace FCSP.Repositories;

public interface IGenericRepository<T> where T : BaseEntity
{
    public Task<T> AddAsync(T entity);
}
