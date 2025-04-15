using FCSP.DTOs;
using FCSP.DTOs.Manufacturer;
using FCSP.Models.Entities;

namespace FCSP.Services.ManufacturerService
{
    public interface IManufacturerService
    {
        Task<BaseResponseModel<List<GetManufacturerDetailResponse>>> GetAllManufacturers();
        Task<BaseResponseModel<GetManufacturerDetailResponse>> GetManufacturerById(GetManufacturerRequest request);
        Task<BaseResponseModel<AddManufacturerResponse>> AddManufacturer(AddManufacturerRequest request);
        Task<BaseResponseModel<UpdateManufacturerResponse>> UpdateManufacturer(UpdateManufacturerRequest request);
        Task<BaseResponseModel<bool>> DeleteManufacturer(GetManufacturerRequest request);
        Task<BaseResponseModel<List<GetManufacturerDetailResponse>>> GetManufacturersByUserId(long userId);
        Task<BaseResponseModel<List<GetManufacturerDetailResponse>>> GetActiveManufacturers();
    }
}