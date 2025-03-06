using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;

namespace FCSP.Repositories.Implementations;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(FcspDbContext context) : base(context)
    {
    }
}
