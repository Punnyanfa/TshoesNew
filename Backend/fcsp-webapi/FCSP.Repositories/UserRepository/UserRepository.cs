using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FCSP.Repositories.Implementations;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(FcspDbContext context) : base(context)
    {
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await Entities
                        .Include(u => u.Designers)
                        .Include(u => u.Manufacturers)
                        .OrderByDescending(u => u.CreatedAt)
                        .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
    }

    public async Task<User> GetEmailByUserIdAsync(long userId)
    {
        return await Entities
            .Where(u => u.Id == userId && !u.IsDeleted)
            .OrderByDescending(u => u.CreatedAt)
            .Select(u => new User
            {
                Email = u.Email
            })
            .FirstOrDefaultAsync();
    }

    public async Task<User> GetUserNameByUserIdAsync(long userId)
    {
        return await Entities
            .Where(u => u.Id == userId && !u.IsDeleted)
            .OrderByDescending(u => u.CreatedAt) // Order by the original entity's CreatedAt
            .Select(u => new User
            {
                Name = u.Name
            })
            .FirstOrDefaultAsync();
    }

    public async Task<User?> GetByIdAsync(long userId)
    {
        return await Entities
            .OrderByDescending(u => u.CreatedAt)
            .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);
    }
}