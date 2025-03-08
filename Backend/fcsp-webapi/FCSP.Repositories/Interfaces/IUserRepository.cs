using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;

namespace FCSP.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User> GetByEmailAsync(string email);
}
