using FCSP.DTOs.Design;
using FCSP.Models.Entities;

namespace FCSP.Services.DesignService
{
    public interface IDesignService
    {
        Task<IEnumerable<CustomShoeDesign>> GetAllDesign();
        Task<GetDesignByIdResponse> GetDesignById(GetDesignByIdRequest request);
        Task<GetDesignByIdResponse> AddDesign(AddDesignRequest request);
        Task<GetDesignByIdResponse> UpdateDesign(UpdateDesignRequest request);
        Task<GetDesignByIdResponse> DeleteDesign(DeleteDesignRequest request);
    }
}
