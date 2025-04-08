using FCSP.Models.Entities;

namespace FCSP.Repositories.Interfaces
{
    public interface ICustomShoeDesignRepository : IGenericRepository<CustomShoeDesign>
    {
        Task<CustomShoeDesign> FindByIdAsync(long id);
        Task<IEnumerable<CustomShoeDesign>> GetDesignsByUserIdAsync(long userId);
        Task<IEnumerable<CustomShoeDesign>> GetAllPublicCustomShoeDesignsAsync();
        Task<IEnumerable<CustomShoeDesign>> GetDesignsByIdsAsync(IEnumerable<long> designIds);
    }
}