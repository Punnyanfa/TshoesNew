using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Implementations;

namespace FCSP.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(FcspDbContext context) : base(context)
    {
    }
}
