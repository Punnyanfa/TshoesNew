using FCSP.DTOs;
using FCSP.DTOs.Rating;
using FCSP.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCSP.Services.RatingService
{
    public interface IRatingService
    {
        Task<BaseResponseModel<IEnumerable<Rating>>> GetAllRatings();
        Task<BaseResponseModel<GetRatingByIdResponse>> GetRatingById(GetRatingByIdRequest request);
        Task<BaseResponseModel<AddRatingResponse>> AddRating(AddRatingRequest request);
        Task<BaseResponseModel<UpdateRatingResponse>> UpdateRating(UpdateRatingRequest request);
        Task<BaseResponseModel<DeleteRatingResponse>> DeleteRating(DeleteRatingRequest request);
        Task<BaseResponseModel<List<CustomShoeRatingStats>>> GetCustomShoeRatingStats();
        Task<BaseResponseModel<List<TopRatedCustomShoe>>> GetTopRatedCustomShoes();
        Task<BaseResponseModel<CustomShoeRatingStats>> GetCustomShoeRatingStatsById(long customShoeDesignId);
    }
}