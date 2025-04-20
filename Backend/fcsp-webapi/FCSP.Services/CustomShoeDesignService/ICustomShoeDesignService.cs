using FCSP.DTOs;
using FCSP.DTOs.CustomShoeDesign;

namespace FCSP.Services.CustomShoeDesignService
{
    public interface ICustomShoeDesignService
    {
        Task<BaseResponseModel<GetAllPublicCustomShoeDesignsResponse>> GetAllPublicDesigns();
        Task<BaseResponseModel<GetAllPublicCustomShoeDesignsResponse>> GetTopFiveBestSellingPublicDesigns();
        Task<BaseResponseModel<GetCustomShoeDesignByIdResponse>> GetDesignById(GetCustomShoeDesignByIdRequest request);
        Task<BaseResponseModel<GetListCustomShoeDesignsResponse>> GetDesignsByUserId(GetCustomShoeDesignsByUserIdRequest request);
        Task<BaseResponseModel<AddCustomShoeDesignResponse>> AddCustomShoeDesign(AddCustomShoeDesignRequest request);
        Task<BaseResponseModel<UpdateCustomShoeDesignStatusResponse>> UpdateCustomShoeDesignStatus(UpdateCustomShoeDesignStatusRequest request);
        Task<BaseResponseModel<UpdateCustomShoeDesignResponse>> UpdateCustomShoeDesign(UpdateCustomShoeDesignRequest request);
        Task<BaseResponseModel<DeleteCustomShoeDesignResponse>> DeleteCustomShoeDesign(DeleteCustomShoeDesignRequest request);
    }
}