using FCSP.DTOs.CustomShoeDesign;
using FCSP.Models.Entities;

namespace FCSP.Services.CustomShoeDesignService
{
    public interface ICustomShoeDesignService
    {
        Task<IEnumerable<GetCustomShoeDesignByIdResponse>> GetAllCustomShoeDesigns();
        Task<GetCustomShoeDesignByIdResponse> GetCustomShoeDesignById(GetCustomShoeDesignByIdRequest request);
        Task<IEnumerable<GetCustomShoeDesignByIdResponse>> GetDesignsByUser(GetDesignsByUserRequest request);
        Task<AddCustomShoeDesignResponse> AddCustomShoeDesign(AddCustomShoeDesignRequest request);
        Task<UpdateCustomShoeDesignResponse> UpdateCustomShoeDesign(UpdateCustomShoeDesignRequest request);
        Task<DeleteCustomShoeDesignResponse> DeleteCustomShoeDesign(DeleteCustomShoeDesignRequest request);
        
        // Methods for handling CustomShoeDesignTexture
        Task<IEnumerable<CustomShoeDesignTexture>> GetDesignTextures(long designId);
        Task<CustomShoeDesignTexture> AddTextureToDesign(long designId, long textureId);
        Task RemoveTextureFromDesign(long designId, long textureId);
        Task RemoveAllTexturesFromDesign(long designId);
    }
} 