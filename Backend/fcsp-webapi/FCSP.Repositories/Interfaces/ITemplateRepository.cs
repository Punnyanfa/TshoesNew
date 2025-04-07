using FCSP.Models.Entities;

namespace FCSP.Repositories.Interfaces
{
    public interface ICustomShoeDesignTemplateRepository : IGenericRepository<CustomShoeDesignTemplate>
    {
        Task<IEnumerable<long>> GetCustomShoeDesignIdsByTemplateAsync(long templateId);
        Task<int> GetCustomShoeDesignCountByTemplateAsync(long templateId);
    }
}
