using FCSP.DTOs;
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

        #region Public Methods

        public async Task<BaseResponseModel<IEnumerable<Rating>>> GetAllRatings()
        {
            try
            {
                var ratings = await _ratingRepository.GetAllAsync();
                return new BaseResponseModel<IEnumerable<Rating>>
                {
                    Code = 200,
                    Message = "Ratings retrieved successfully",
                    Data = ratings
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<IEnumerable<Rating>>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<GetRatingByIdResponse>> GetRatingById(GetRatingByIdRequest request)
        {
            try
            {
                var rating = await GetRatingByIdInternal(request);
                return new BaseResponseModel<GetRatingByIdResponse>
                {
                    Code = 200,
                    Message = "Rating retrieved successfully",
                    Data = MapToDetailedResponse(rating)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<GetRatingByIdResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<AddRatingResponse>> AddRating(AddRatingRequest request)
        {
            try
            {
                var rating = await AddRatingInternal(request);
                return new BaseResponseModel<AddRatingResponse>
                {
                    Code = 200,
                    Message = "Rating added successfully",
                    Data = new AddRatingResponse { RatingId = rating.Id }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<AddRatingResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = new AddRatingResponse { RatingId = 0 }
                };
            }
        }

        public async Task<BaseResponseModel<UpdateRatingResponse>> UpdateRating(UpdateRatingRequest request)
        {
            try
            {
                var rating = await UpdateRatingInternal(request);
                return new BaseResponseModel<UpdateRatingResponse>
                {
                    Code = 200,
                    Message = "Rating updated successfully",
                    Data = new UpdateRatingResponse { RatingId = rating.Id }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<UpdateRatingResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = new UpdateRatingResponse { RatingId = 0 }
                };
            }
        }

        public async Task<BaseResponseModel<DeleteRatingResponse>> DeleteRating(DeleteRatingRequest request)
        {
            try
            {
                await DeleteRatingInternal(request);
                return new BaseResponseModel<DeleteRatingResponse>
                {
                    Code = 200,
                    Message = "Rating deleted successfully",
                    Data = new DeleteRatingResponse { Success = true }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<DeleteRatingResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = new DeleteRatingResponse { Success = false }
                };
            }
        }

        public async Task<BaseResponseModel<List<CustomShoeRatingStats>>> GetCustomShoeRatingStats()
        {
            try
            {
                var stats = await GetCustomShoeRatingStatsInternal();
                return new BaseResponseModel<List<CustomShoeRatingStats>>
                {
                    Code = 200,
                    Message = "Rating stats retrieved successfully",
                    Data = stats
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<List<CustomShoeRatingStats>>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<List<TopRatedCustomShoe>>> GetTopRatedCustomShoes()
        {
            try
            {
                var topRated = await GetTopRatedCustomShoesInternal();
                return new BaseResponseModel<List<TopRatedCustomShoe>>
                {
                    Code = 200,
                    Message = "Top rated custom shoes retrieved successfully",
                    Data = topRated
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<List<TopRatedCustomShoe>>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<CustomShoeRatingStats>> GetCustomShoeRatingStatsById(long customShoeDesignId)
        {
            try
            {
                var stats = await GetCustomShoeRatingStatsByIdInternal(customShoeDesignId);
                return new BaseResponseModel<CustomShoeRatingStats>
                {
                    Code = 200,
                    Message = "Rating stats for custom shoe design retrieved successfully",
                    Data = stats
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<CustomShoeRatingStats>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        #endregion

        #region Private Methods

        private async Task<Rating> GetRatingByIdInternal(GetRatingByIdRequest request)
        {
            var rating = await _ratingRepository.FindAsync(request.Id);
            if (rating == null)
            {
                throw new InvalidOperationException($"Rating with ID {request.Id} not found");
            }
            return rating;
        }

        private async Task<Rating> AddRatingInternal(AddRatingRequest request)
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
            return await _ratingRepository.AddAsync(rating);
        }

        private async Task<Rating> UpdateRatingInternal(UpdateRatingRequest request)
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
            return rating;
        }

        private async Task DeleteRatingInternal(DeleteRatingRequest request)
        {
            var rating = await _ratingRepository.FindAsync(request.Id);
            if (rating == null)
            {
                throw new InvalidOperationException($"Rating with ID {request.Id} not found");
            }
            await _ratingRepository.DeleteAsync(request.Id);
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

        private async Task<List<CustomShoeRatingStats>> GetCustomShoeRatingStatsInternal()
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

        private async Task<List<TopRatedCustomShoe>> GetTopRatedCustomShoesInternal()
        {
            var ratings = await _ratingRepository.GetAllAsync();
            return ratings
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
        }

        private async Task<CustomShoeRatingStats> GetCustomShoeRatingStatsByIdInternal(long customShoeDesignId)
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

            for (int i = 1; i <= 5; i++)
            {
                if (!stats.RatingBreakdown.ContainsKey(i))
                {
                    stats.RatingBreakdown[i] = 0;
                }
            }

            return stats;
        }

        #endregion
    }
}