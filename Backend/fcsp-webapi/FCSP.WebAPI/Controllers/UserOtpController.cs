using FCSP.DTOs.UserOtp;
using FCSP.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCSP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserOtpController : ControllerBase
    {
        private readonly IAuthService _authService;

        public UserOtpController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("generate")]
        [Authorize]
        public async Task<IActionResult> GenerateOtp([FromBody] GenerateOtpRequest request)
        {
            var response = await _authService.GenerateOtpAsync(request);
            return StatusCode(response.Code, response);
        }

        [HttpPost("verify")]
        [Authorize]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            var response = await _authService.VerifyOtpAsync(request);
            return StatusCode(response.Code, response);
        }
    }
} 