using FCSP.Models.Entities;

namespace FCSP.Repositories.Interfaces
{
    public interface ICriteriaRepository : IGenericRepository<Criteria>
    {
        Task<IEnumerable<Criteria>> GetActiveCriteriaAsync();
    }
} 