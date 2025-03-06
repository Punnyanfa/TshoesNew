using FCSP.DTOs.DesignPreview;
using FCSP.Models.Entities;

namespace FCSP.Services.DesignPreviewService
{
    public interface IDesignPreviewService
    {
        Task<IEnumerable<GetDesignPreviewByIdResponse>> GetAllDesignPreviews();
        Task<GetDesignPreviewByIdResponse> GetDesignPreviewById(GetDesignPreviewByIdRequest request);
        Task<IEnumerable<GetDesignPreviewByIdResponse>> GetPreviewsByDesign(GetPreviewsByDesignRequest request);
        Task<AddDesignPreviewResponse> AddDesignPreview(AddDesignPreviewRequest request);
        Task<UpdateDesignPreviewResponse> UpdateDesignPreview(UpdateDesignPreviewRequest request);
        Task<DeleteDesignPreviewResponse> DeleteDesignPreview(DeleteDesignPreviewRequest request);
    }
} 