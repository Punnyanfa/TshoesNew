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
        return await Entities.FirstOrDefaultAsync(u => u.Email == email && (!u.IsDeleted || !u.IsBanned));
    }

    public async Task<User> GetEmailByUserIdAsync(long userId)
    {
        return await Entities.Where(u => u.Id == userId && !u.IsDeleted)
            .Select(u => new User
            {
                Email = u.Email
            })
            .FirstOrDefaultAsync();
    }

    public async Task<User> GetUserNameByUserIdAsync(long userId)
    {
        return await Entities.Where(u => u.Id == userId && !u.IsDeleted)
            .Select(u => new User
            {
                Name = u.Name
            })
            .FirstOrDefaultAsync();
    }

    public async Task<User?> GetByIdAsync(long userId)
    {
        return await Entities.FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);
    }
}
