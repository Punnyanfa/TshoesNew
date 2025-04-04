using FCSP.DTOs.UserActivity;
using FCSP.Services.UserActivityService;
using Microsoft.AspNetCore.Mvc;

namespace FCSP.WebAPI.Controllers.UserActivity
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserActivityController : ControllerBase
    {
        private readonly IUserActivityService _userActivityService;

        public UserActivityController(IUserActivityService userActivityService)
        {
            _userActivityService = userActivityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserActivities()
        {
            var activities = await _userActivityService.GetAllUserActivities();
            return Ok(activities);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserActivityById(long id)
        {
            var request = new GetUserActivityByIdRequest { Id = id };
            var activity = await _userActivityService.GetUserActivityById(request);
            return Ok(activity);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetActivitiesByUser(long userId)
        {
            var request = new GetActivitiesByUserRequest { UserId = userId };
            var activities = await _userActivityService.GetActivitiesByUser(request);
            return Ok(activities);
        }

        [HttpGet("design/{designId}")]
        public async Task<IActionResult> GetActivitiesByDesign(long designId)
        {
            var request = new GetActivitiesByDesignRequest { DesignId = designId };
            var activities = await _userActivityService.GetActivitiesByDesign(request);
            return Ok(activities);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserActivity([FromBody] AddUserActivityRequest request)
        {
            var response = await _userActivityService.AddUserActivity(request);
            return StatusCode(response.Code, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserActivity(long id, [FromBody] UpdateUserActivityRequest request)
        {
            if (id != request.Id)
                return BadRequest();

            var response = await _userActivityService.UpdateUserActivity(request);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserActivity(long id)
        {
            var request = new DeleteUserActivityRequest { Id = id };
            var response = await _userActivityService.DeleteUserActivity(request);
            return Ok(response);
        }
    }
} 