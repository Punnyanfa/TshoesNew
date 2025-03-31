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
        public async Task<List<CustomShoeRatingStats>> GetCustomShoeRatingStats()
        {
            var ratings = await _ratingRepository.GetAllAsync();
            var groupedRatings = ratings
                .Where(r => !r.IsDeleted)
                .GroupBy(r => r.CustomShoeDesignId)
                .Select(g => new CustomShoeRatingStats
                {
                    CustomShoeDesignId = g.Key,
                    RatingBreakdown = g.GroupBy(r => r.UserRating)
                        .ToDictionary(
                            r => r.Key,
                            r => r.Count()
                        )
                }).ToList();

            // Đảm bảo tất cả các mức rating từ 1-5 đều có trong breakdown
            foreach (var stat in groupedRatings)
            {
                for (int i = 1; i <= 5; i++)
                {
                    if (!stat.RatingBreakdown.ContainsKey(i))
                    {
                        stat.RatingBreakdown[i] = 0;
                    }
                }
            }

            return groupedRatings;
        }

        public async Task<List<TopRatedCustomShoe>> GetTopRatedCustomShoes()
        {
            var ratings = await _ratingRepository.GetAllAsync();
            var topRated = ratings
                .Where(r => !r.IsDeleted && r.UserRating >= 4)
                .GroupBy(r => r.CustomShoeDesignId)
                .Select(g => new TopRatedCustomShoe
                {
                    CustomShoeDesignId = g.Key,
                    HighRatingCount = g.Count()
                })
                .OrderByDescending(x => x.HighRatingCount)
                .Take(3)
                .ToList();

            return topRated;
        }
        public async Task<CustomShoeRatingStats> GetCustomShoeRatingStatsById(long customShoeDesignId)
        {
            var ratings = await _ratingRepository.GetAllAsync();
            var filteredRatings = ratings
                .Where(r => !r.IsDeleted && r.CustomShoeDesignId == customShoeDesignId)
                .ToList();

            if (!filteredRatings.Any())
            {
                throw new InvalidOperationException($"No ratings found for CustomShoeDesignId {customShoeDesignId}");
            }

            var stats = new CustomShoeRatingStats
            {
                CustomShoeDesignId = customShoeDesignId,
                RatingBreakdown = filteredRatings
                    .GroupBy(r => r.UserRating)
                    .ToDictionary(
                        r => r.Key,
                        r => r.Count()
                    )
            };

            // Đảm bảo tất cả mức rating từ 1-5 đều có trong breakdown
            for (int i = 1; i <= 5; i++)
            {
                if (!stats.RatingBreakdown.ContainsKey(i))
                {
                    stats.RatingBreakdown[i] = 0;
                }
            }

            return stats;
        }
    }
} 