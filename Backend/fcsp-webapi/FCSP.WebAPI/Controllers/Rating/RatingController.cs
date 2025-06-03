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
            return StatusCode(ratings.Code, ratings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRatingById(long id)
        {
            var request = new GetRatingByIdRequest { Id = id };
            var rating = await _ratingService.GetRatingById(request);
            return StatusCode(rating.Code, rating);
        }

        [HttpPost]
        public async Task<IActionResult> AddRating([FromBody] AddRatingRequest request)
        {
            var response = await _ratingService.AddRating(request);
            if (response.Code != 200)
            {
                return StatusCode(response.Code, response.Message);
            }
            return StatusCode(response.Code, response.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRating(long id, [FromBody] UpdateRatingRequest request)
        {
            if (id != request.Id)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _ratingService.UpdateRating(request);
            return StatusCode(response.Code, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRating(long id)
        {
            var request = new DeleteRatingRequest { Id = id };
            var response = await _ratingService.DeleteRating(request);
            return StatusCode(response.Code, response);
        }

        [HttpGet("top-rated")]
        public async Task<IActionResult> GetTopRatedCustomShoes()
        {
            var topRated = await _ratingService.GetTopRatedCustomShoes();
            return StatusCode(topRated.Code, topRated);
        }

        [HttpGet("design/{id}")]
        public async Task<IActionResult> GetCustomShoeRatingStatsById([FromRoute] long id)
        {
            try
            {
                var request = new GetRatingsByCustomShoeDesignIdRequest { CustomShoeDesignId = id }; 
                var response = await _ratingService.GetRatingsByCustomShoeDesignId(request);
                return StatusCode(response.Code, response);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}