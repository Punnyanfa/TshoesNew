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
            entity.UpdatedAt = DateTime.UtcNow;
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await Entities.FindAsync(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                Entities.Update(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IList<T>> GetAllAsync()
        {
            return await Entities
                .Where(x => !x.IsDeleted)
                .ToListAsync();
        }

        public IQueryable<T> GetAll()
            => Entities.Where(x => true).AsQueryable();

        public T Find(params object[] keyValues)
        {
            return Entities.Find(keyValues);
        }

        public async Task<T> FindAsync(params object[] keyValues)
        {
            return await Entities.FindAsync(keyValues);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }
    }
}