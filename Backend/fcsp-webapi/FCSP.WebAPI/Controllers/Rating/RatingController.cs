using FCSP.DTOs.Rating;
using FCSP.Services.RatingService;
using Microsoft.AspNetCore.Mvc;

namespace FCSP.WebAPI.Controllers.Rating
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRatings()
        {
            var ratings = await _ratingService.GetAllRatings();
            return Ok(ratings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRatingById(long id)
        {
            var request = new GetRatingByIdRequest { Id = id };
            var rating = await _ratingService.GetRatingById(request);
            return Ok(rating);
        }

        [HttpPost]
        public async Task<IActionResult> AddRating([FromBody] AddRatingRequest request)
        {
            var response = await _ratingService.AddRating(request);
            return CreatedAtAction(nameof(GetRatingById), new { id = response.RatingId }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRating(long id, [FromBody] UpdateRatingRequest request)
        {
            if (id != request.Id)
                return BadRequest();

            var response = await _ratingService.UpdateRating(request);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRating(long id)
        {
            var request = new DeleteRatingRequest { Id = id };
            var response = await _ratingService.DeleteRating(request);
            return Ok(response);
        }
    }
} 