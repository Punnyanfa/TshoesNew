using FCSP.DTOs.Rating;
using FCSP.Models.Entities;

namespace FCSP.Services.RatingService
{
    public interface IRatingService
    {
        Task<IEnumerable<Rating>> GetAllRatings();
        Task<GetRatingByIdResponse> GetRatingById(GetRatingByIdRequest request);
        Task<AddRatingResponse> AddRating(AddRatingRequest request);
        Task<UpdateRatingResponse> UpdateRating(UpdateRatingRequest request);
        Task<DeleteRatingResponse> DeleteRating(DeleteRatingRequest request);
    }
} 