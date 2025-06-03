using FCSP.DTOs;
using FCSP.DTOs.Rating;

namespace FCSP.Services.RatingService
{
    public interface IRatingService
    {
        Task<BaseResponseModel<IEnumerable<GetRatingByIdResponse>>> GetAllRatings();
        Task<BaseResponseModel<GetRatingByIdResponse>> GetRatingById(GetRatingByIdRequest request);
        Task<BaseResponseModel<AddRatingResponse>> AddRating(AddRatingRequest request);
        Task<BaseResponseModel<UpdateRatingResponse>> UpdateRating(UpdateRatingRequest request);
        Task<BaseResponseModel<DeleteRatingResponse>> DeleteRating(DeleteRatingRequest request);
        Task<BaseResponseModel<List<TopRatedCustomShoe>>> GetTopRatedCustomShoes();
        Task<BaseResponseModel<GetRatingsByCustomShoeDesignIdResponse>> GetRatingsByCustomShoeDesignId(GetRatingsByCustomShoeDesignIdRequest request);
    }
}