using FCSP.Models.Context;
using FCSP.Models.Entities;

namespace FCSP.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(FcspDbContext context) : base(context)
    {
    }
}
