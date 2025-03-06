using FCSP.DTOs.Notification;
using FCSP.Services.NotificationService;
using Microsoft.AspNetCore.Mvc;

namespace FCSP.WebAPI.Controllers.Notification
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotifications()
        {
            var notifications = await _notificationService.GetAllNotifications();
            return Ok(notifications);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotificationById(long id)
        {
            var request = new GetNotificationByIdRequest { Id = id };
            var notification = await _notificationService.GetNotificationById(request);
            return Ok(notification);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetNotificationsByUser(long userId)
        {
            var request = new GetNotificationsByUserRequest { UserId = userId };
            var notifications = await _notificationService.GetNotificationsByUser(request);
            return Ok(notifications);
        }

        [HttpPost]
        public async Task<IActionResult> AddNotification([FromBody] AddNotificationRequest request)
        {
            var response = await _notificationService.AddNotification(request);
            return CreatedAtAction(nameof(GetNotificationById), new { id = response.NotificationId }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotification(long id, [FromBody] UpdateNotificationRequest request)
        {
            if (id != request.Id)
                return BadRequest();

            var response = await _notificationService.UpdateNotification(request);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(long id)
        {
            var request = new DeleteNotificationRequest { Id = id };
            var response = await _notificationService.DeleteNotification(request);
            return Ok(response);
        }
    }
} 