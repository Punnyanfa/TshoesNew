using FCSP.DTOs.CustomShoeDesign;
using FCSP.Models.Entities;
using FCSP.DTOs;

namespace FCSP.Services.CustomShoeDesignService
{
    public interface ICustomShoeDesignService
    {
        Task<BaseResponseModel<GetAllPublicCustomShoeDesignsResponse>> GetAllPublicDesigns();
        Task<BaseResponseModel<GetCustomShoeDesignByIdResponse>> GetDesignById(GetCustomShoeDesignByIdRequest request);
        Task<BaseResponseModel<GetCustomShoeDesignsByUserIdResponse>> GetDesignsByUserId(GetCustomShoeDesignsByUserIdRequest request);
        Task<BaseResponseModel<AddCustomShoeDesignResponse>> AddCustomShoeDesign(AddCustomShoeDesignRequest request);
        Task<BaseResponseModel<UpdateCustomShoeDesignResponse>> UpdateCustomShoeDesign(UpdateCustomShoeDesignRequest request);
        Task<BaseResponseModel<DeleteCustomShoeDesignResponse>> DeleteCustomShoeDesign(DeleteCustomShoeDesignRequest request);
    }
} 