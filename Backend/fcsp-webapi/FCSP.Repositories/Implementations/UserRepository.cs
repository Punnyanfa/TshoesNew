using FCSP.Models.Context;
using FCSP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FCSP.Repositories.Implementations;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(FcspDbContext context) : base(context)
    {
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await Entities.FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
    }
}
