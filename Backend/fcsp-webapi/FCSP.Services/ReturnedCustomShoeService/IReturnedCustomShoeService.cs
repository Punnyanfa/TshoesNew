using FCSP.DTOs;
using FCSP.DTOs.ReturnedCustomShoe;
using System.Threading.Tasks;

namespace FCSP.Services.ReturnedCustomShoeService
{
    public interface IReturnedCustomShoeService
    {
        Task<BaseResponseModel<AddReturnedCustomShoeResponse>> AddReturnedCustomShoe(AddReturnedCustomShoeRequest request);
        Task<BaseResponseModel<GetReturnedCustomShoesResponse>> GetAllReturnedCustomShoes();
        Task<BaseResponseModel<GetReturnedCustomShoeByIdResponse>> GetReturnedCustomShoeById(GetReturnedCustomShoeByIdRequest request);
        Task<BaseResponseModel<GetReturnedCustomShoesResponse>> GetReturnedCustomShoesByDesignId(GetReturnedCustomShoeByDesignIdRequest request);
    }
} 