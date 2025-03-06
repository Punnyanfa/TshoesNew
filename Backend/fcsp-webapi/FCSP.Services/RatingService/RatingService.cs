using FCSP.DTOs.Rating;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCSP.Services.RatingService
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;

        public RatingService(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public async Task<IEnumerable<Rating>> GetAllRatings()
        {
            return await _ratingRepository.GetAllAsync();
        }

        public async Task<GetRatingByIdResponse> GetRatingById(GetRatingByIdRequest request)
        {
            var rating = await _ratingRepository.FindAsync(request.Id);
            if (rating == null)
            {
                throw new InvalidOperationException($"Rating with ID {request.Id} not found");
            }

            return MapToDetailedResponse(rating);
        }

        public async Task<AddRatingResponse> AddRating(AddRatingRequest request)
        {
            var rating = new Rating
            {
                UserId = request.UserId,
                CustomShoeDesignId = request.TargetId,
                UserRating = request.Value,
                Comment = request.Comment,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var addedRating = await _ratingRepository.AddAsync(rating);
            return new AddRatingResponse { RatingId = addedRating.Id };
        }

        public async Task<UpdateRatingResponse> UpdateRating(UpdateRatingRequest request)
        {
            var rating = await _ratingRepository.FindAsync(request.Id);
            if (rating == null)
            {
                throw new InvalidOperationException($"Rating with ID {request.Id} not found");
            }

            rating.UserRating = request.Value;
            rating.Comment = request.Comment;
            rating.UpdatedAt = DateTime.UtcNow;

            await _ratingRepository.UpdateAsync(rating);
            return new UpdateRatingResponse { RatingId = rating.Id };
        }

        public async Task<DeleteRatingResponse> DeleteRating(DeleteRatingRequest request)
        {
            var rating = await _ratingRepository.FindAsync(request.Id);
            if (rating == null)
            {
                throw new InvalidOperationException($"Rating with ID {request.Id} not found");
            }

            await _ratingRepository.DeleteAsync(request.Id);
            return new DeleteRatingResponse { Success = true };
        }

        private GetRatingByIdResponse MapToDetailedResponse(Rating rating)
        {
            return new GetRatingByIdResponse
            {
                Id = rating.Id,
                UserId = rating.UserId,
                TargetId = rating.CustomShoeDesignId,
                Type = "CustomShoeDesign",
                Value = rating.UserRating,
                Comment = rating.Comment ?? string.Empty,
                CreatedAt = rating.CreatedAt,
                UpdatedAt = rating.UpdatedAt
            };
        }
    }
} 