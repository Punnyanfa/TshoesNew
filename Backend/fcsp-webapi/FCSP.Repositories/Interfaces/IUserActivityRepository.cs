using FCSP.Models.Entities;

namespace FCSP.Repositories.Interfaces
{
    public interface IUserActivityRepository : IGenericRepository<UserActivity>
    {
        Task<IEnumerable<UserActivity>> GetActivitiesByUserIdAsync(long userId);
        Task<IEnumerable<UserActivity>> GetActivitiesByDesignIdAsync(long designId);
    }
}