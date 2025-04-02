using FCSP.Models.Entities;

namespace FCSP.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        public Task<T> AddAsync(T entity);
        public Task UpdateAsync(T entity);
        public Task DeleteAsync(long id);
        public Task<IList<T>> GetAllAsync();
        public IQueryable<T> GetAll();
        public T Find(params object[] keyValues);
        public Task<T> FindAsync(params object[] keyValues);
    }
}