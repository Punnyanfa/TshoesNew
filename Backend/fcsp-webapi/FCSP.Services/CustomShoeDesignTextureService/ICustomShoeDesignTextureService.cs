using FCSP.DTOs.CustomShoeDesignTexture;
using FCSP.Models.Entities;

namespace FCSP.Services.CustomShoeDesignTextureService
{
    public interface ICustomShoeDesignTextureService
    {
        Task<IEnumerable<CustomShoeDesignTexture>> GetAllCustomShoeDesignTextures();
        Task<GetCustomShoeDesignTextureByIdResponse> GetCustomShoeDesignTextureById(GetCustomShoeDesignTextureByIdRequest request);
        Task<AddCustomShoeDesignTextureResponse> AddCustomShoeDesignTexture(AddCustomShoeDesignTextureRequest request);
        Task<AddCustomShoeDesignTextureResponse> UpdateCustomShoeDesignTexture(UpdateCustomShoeDesignTextureRequest request);
        Task<AddCustomShoeDesignTextureResponse> DeleteCustomShoeDesignTexture(DeleteCustomShoeDesignTextureRequest request);
    }
} 