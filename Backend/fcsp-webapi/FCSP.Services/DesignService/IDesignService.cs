using FCSP.DTOs.Design;
using FCSP.Models.Entities;

namespace FCSP.Services.DesignService
{
    public interface IDesignService
    {
        Task<IEnumerable<CustomShoeDesign>> GetAllTemplate();
        Task<GetDesignByIdResponse> GetTemplateById(GetDesignByIdRequest request);
        Task<GetDesignByIdResponse> AddDesign(AddDesignRequest request);
        Task<GetDesignByIdResponse> UpdateDesign(UpdateDesignRequest request);
        Task<GetDesignByIdResponse> DeleteDesign(DeleteDesignRequest request);
    }
}
