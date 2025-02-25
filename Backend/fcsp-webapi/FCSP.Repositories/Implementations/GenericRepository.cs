using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FCSP.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly FcspDbContext _context;
        public DbSet<T> Entities => _context.Set<T>();

        public GenericRepository(FcspDbContext context)
        {
            _context = context;
        }

        public async Task<T> AddAsync(T entity)
        {
            T newEntity = _context.Add(entity).Entity;
            await _context.SaveChangesAsync();

            return newEntity;
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await Entities.FindAsync(id);
            Entities.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<T>> GetAllAsync()
        {
            return await Entities.ToListAsync();
        }

        public T Find(params object[] keyValues)
        {
            return Entities.Find(keyValues);
        }

        public async Task<T> FindAsync(params object[] keyValues)
        {
            return await Entities.FindAsync(keyValues);
        }
    }
} 