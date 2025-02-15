using FCSP.Models.Context;
using FCSP.Models.Entities;

namespace FCSP.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly FcspDbContext _context;

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
}
