using FCSP.Models.Entities;

namespace FCSP.Repositories.Interfaces
{
    public interface IDesignPreviewRepository : IGenericRepository<DesignPreview>
    {
        Task<IEnumerable<DesignPreview>> GetPreviewsByCustomShoeDesignIdAsync(long customShoeDesignId);
    }
} 