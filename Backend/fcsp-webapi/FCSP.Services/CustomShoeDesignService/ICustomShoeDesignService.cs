using FCSP.DTOs.CustomShoeDesign;
using FCSP.Models.Entities;

namespace FCSP.Services.CustomShoeDesignService
{
    public interface ICustomShoeDesignService
    {
        Task<IEnumerable<CustomShoeDesign>> GetAllCustomShoeDesigns();
        Task<GetCustomShoeDesignByIdResponse> GetCustomShoeDesignById(GetCustomShoeDesignByIdRequest request);
        Task<GetCustomShoeDesignByIdResponse> AddCustomShoeDesign(AddCustomShoeDesignRequest request);
        Task<GetCustomShoeDesignByIdResponse> UpdateCustomShoeDesign(UpdateCustomShoeDesignRequest request);
        Task<GetCustomShoeDesignByIdResponse> DeleteCustomShoeDesign(DeleteCustomShoeDesignRequest request);
        
        // Methods for handling CustomShoeDesignTexture
        Task<IEnumerable<CustomShoeDesignTexture>> GetDesignTextures(long designId);
        Task<CustomShoeDesignTexture> AddTextureToDesign(long designId, long textureId);
        Task RemoveTextureFromDesign(long designId, long textureId);
        Task RemoveAllTexturesFromDesign(long designId);
    }
} 