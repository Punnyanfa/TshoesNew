using FCSP.DTOs;
using FCSP.DTOs.Rating;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using FCSP.Common.Utils;

namespace FCSP.Services.RatingService
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly ICustomShoeDesignRepository _customShoeDesignRepository;
        private readonly IOrderRepository _orderRepository;

        public RatingService(
            IRatingRepository ratingRepository,
            ICustomShoeDesignRepository customShoeDesignRepository,
            IOrderRepository orderRepository)
        {
            _ratingRepository = ratingRepository;
            _customShoeDesignRepository = customShoeDesignRepository;
            _orderRepository = orderRepository;
        }

        #region Public Methods

        public async Task<BaseResponseModel<IEnumerable<GetRatingByIdResponse>>> GetAllRatings()
        {
            try
            {
                var ratings = await _ratingRepository.GetAll()
                    .Where(r => !r.IsDeleted) // Lọc các bản ghi chưa bị xóa mềm
                    .Include(r => r.User)
                    .Include(r => r.CustomShoeDesign)
                        .ThenInclude(r => r.DesignPreviews)
                    .ToListAsync();

                return new BaseResponseModel<IEnumerable<GetRatingByIdResponse>>
                {
                    Code = 200,
                    Message = "Ratings retrieved successfully",
                    Data = ratings.Select(d => new GetRatingByIdResponse
                    {
                        Id = d.Id,
                        CustomShoeDesignId = d.CustomShoeDesignId,
                        DesignName = d.CustomShoeDesign.Name,
                        DesignPreviewUrl = d.CustomShoeDesign.DesignPreviews?.FirstOrDefault()?.PreviewImageUrl ?? string.Empty,
                        UserName = d.User.Name,
                        UserRating = d.UserRating,
                        Comment = d.Comment,
                        CreatedAt = d.CreatedAt
                    })
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<IEnumerable<GetRatingByIdResponse>>
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
                var rating = await AddRatingAsync(request);
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

        public async Task<BaseResponseModel<GetRatingsByCustomShoeDesignIdResponse>> GetRatingsByCustomShoeDesignId(GetRatingsByCustomShoeDesignIdRequest request)
        {
            try
            {
                var stats = await GetCustomShoeDesignRatings(request);
                return new BaseResponseModel<GetRatingsByCustomShoeDesignIdResponse>
                {
                    Code = 200,
                    Message = "Rating stats for custom shoe design retrieved successfully",
                    Data = stats
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<GetRatingsByCustomShoeDesignIdResponse>
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
            var rating = await _ratingRepository.GetAll()
                .Where(r => !r.IsDeleted) // Lọc bản ghi chưa bị xóa mềm
                .Include(r => r.User)
                .Include(r => r.CustomShoeDesign)
                    .ThenInclude(r => r.DesignPreviews)
                .FirstOrDefaultAsync(r => r.Id == request.Id);
            if (rating == null)
            {
                throw new InvalidOperationException($"Rating with ID {request.Id} not found");
            }
            return rating;
        }

        private async Task<Rating> AddRatingAsync(AddRatingRequest request)
        {
            var customShoeDesign = await _customShoeDesignRepository.FindAsync(request.CustomShoeDesignId);
            if (customShoeDesign == null || customShoeDesign.IsDeleted)
            {
                throw new InvalidOperationException($"Custom shoe design with ID {request.CustomShoeDesignId} not found or has been deleted");
            }

            var rate = await _ratingRepository.GetAll()
                                .Where(r => r.CustomShoeDesignId == request.CustomShoeDesignId)
                                .Where(r => r.UserId == request.UserId)
                                .Where(r => r.IsDeleted == false)
                                .ToListAsync();
            if (rate != null)
            {
                throw new InvalidOperationException($"User {request.UserId} has already rated custom shoe design {request.CustomShoeDesignId}");
            }

            var rating = new Rating
            {
                UserId = request.UserId,
                CustomShoeDesignId = request.CustomShoeDesignId,
                UserRating = request.Value,
                Comment = request.Comment,
                CreatedAt = DateTimeUtils.GetCurrentGmtPlus7(),
                UpdatedAt = DateTimeUtils.GetCurrentGmtPlus7(),
                IsDeleted = false // Đảm bảo mặc định là false khi tạo mới
            };
            return await _ratingRepository.AddAsync(rating);
        }

        private async Task<Rating> UpdateRatingInternal(UpdateRatingRequest request)
        {
            var rating = await _ratingRepository.FindAsync(request.Id);
            if (rating == null || rating.IsDeleted)
            {
                throw new InvalidOperationException($"Rating with ID {request.Id} not found or has been deleted");
            }

            var customShoeDesign = await _customShoeDesignRepository.FindAsync(request.CustomShoeDesignId);
            if (customShoeDesign == null || customShoeDesign.IsDeleted)
            {
                throw new InvalidOperationException($"Custom shoe design with ID {request.CustomShoeDesignId} not found or has been deleted");
            }

            rating.UserRating = request.Value;
            rating.Comment = request.Comment;
            rating.CustomShoeDesignId = request.CustomShoeDesignId;
            rating.UpdatedAt = DateTimeUtils.GetCurrentGmtPlus7();

            await _ratingRepository.UpdateAsync(rating);
            return rating;
        }

        private async Task DeleteRatingInternal(DeleteRatingRequest request)
        {
            var rating = await _ratingRepository.FindAsync(request.Id);
            if (rating == null || rating.IsDeleted)
            {
                throw new InvalidOperationException($"Rating with ID {request.Id} not found or has been deleted");
            }

            // Thực hiện soft delete
            rating.IsDeleted = true;
            rating.UpdatedAt = DateTimeUtils.GetCurrentGmtPlus7();
            await _ratingRepository.UpdateAsync(rating); // Cập nhật thay vì xóa
        }

        private GetRatingByIdResponse MapToDetailedResponse(Rating rating)
        {
            return new GetRatingByIdResponse
            {
                Id = rating.Id,
                UserName = rating.User.Name,
                DesignName = rating.CustomShoeDesign.Name,
                DesignPreviewUrl = rating.CustomShoeDesign.DesignPreviews?.FirstOrDefault()?.PreviewImageUrl ?? string.Empty,
                UserRating = rating.UserRating,
                Comment = rating.Comment ?? string.Empty,
                CreatedAt = rating.CreatedAt,
            };
        }

        private async Task<List<TopRatedCustomShoe>> GetTopRatedCustomShoesInternal()
        {
            var ratings = await _ratingRepository.GetAllAsync();
            return ratings
                .Where(r => !r.IsDeleted && r.UserRating >= 4) // Lọc các bản ghi chưa bị xóa mềm
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

        private async Task<GetRatingsByCustomShoeDesignIdResponse> GetCustomShoeDesignRatings(GetRatingsByCustomShoeDesignIdRequest request)
        {
            var ratings = await _ratingRepository.GetAll()
                                                .Include(r => r.User)
                                                .Where(r => !r.IsDeleted && r.CustomShoeDesignId == request.CustomShoeDesignId)
                                                .ToListAsync(); // Lọc các bản ghi chưa bị xóa mềm

            if (!ratings.Any())
            {
                throw new InvalidOperationException($"No ratings found for CustomShoeDesignId {request.CustomShoeDesignId}");
            }

            var stats = new GetRatingsByCustomShoeDesignIdResponse
            {
                CustomShoeDesignId = request.CustomShoeDesignId,
                Rating = ratings.Select(r => new RatingDto
                {
                    UserId = r.UserId,
                    UserName = r.User.Name,
                    UserRating = r.UserRating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt
                })
            };

            return stats;
        }

        #endregion
    }
}