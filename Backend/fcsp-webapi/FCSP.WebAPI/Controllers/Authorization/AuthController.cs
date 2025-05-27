using FCSP.DTOs;
using FCSP.DTOs.Authentication;
using FCSP.DTOs.UserOtp;
using FCSP.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCSP.WebAPI.Controllers.Authorization;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet("hashed-password/{password}")]
    public async Task<IActionResult> GetHashedPassword(string password)
    {
        var response = _authService.HashPassword(password);
        return StatusCode(200, response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var response = await _authService.GetAllUsers();
        return StatusCode(response.Code, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById([FromRoute] long id)
    {
        var request = new GetUserByIdRequest { Id = id };
        var response = await _authService.GetUserById(request);
        return StatusCode(response.Code, response);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
    {
        var response = await _authService.Login(request);
        return StatusCode(response.Code, response);
    }

    [HttpPost("[action]")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
    {
        var response = await _authService.Register(request);
        return StatusCode(response.Code, response);
    }

    [HttpPost("[action]")]
    [AllowAnonymous]
    public async Task<IActionResult> SendEmailToUser([FromBody] SendEmailRequest request)
    {
        var response = await _authService.SendEmailToUser(request);
        return StatusCode(response.Code, response);
    }

    [HttpPost("[action]")]
    [AllowAnonymous]
    public async Task<IActionResult> SendEmailToAdmin([FromBody] SendEmailRequest request)
    {
        var response = await _authService.SendEmailToAdmin(request);
        return StatusCode(response.Code, response);
    }

    [HttpGet("test")]
    [Authorize]
    public IActionResult TestLogin()
    {
        var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
        return Ok(token);
    }

    [HttpPut("password")]
    [Authorize]
    public async Task<IActionResult> UpdateUserPassword([FromBody] UpdateUserPasswordRequest request)
    {
        var response = await _authService.UpdateUserPassword(request);
        return StatusCode(response.Code, response);
    }

    [HttpPut("information")]
    [Authorize]
    public async Task<IActionResult> UpdateUserInformation([FromBody] UpdateUserInformationRequest request)
    {
        var response = await _authService.UpdateUserInformation(request);
        return StatusCode(response.Code, response);
    }

    [HttpPut("avatar")]
    [Authorize]
    public async Task<IActionResult> UpdateUserAvatar([FromForm] UpdateUserAvatarRequest request)
    {
        var response = await _authService.UpdateUserAvatar(request);
        return StatusCode(response.Code, response);
    }

    [HttpPut("reset-password")]
    [Authorize]
    public async Task<IActionResult> ResetUserPassword([FromBody] ForgetUserPasswordRequest request)
    {
        var response = await _authService.ForgetUserPassword(request);
        return StatusCode(response.Code, response);
    }

    [HttpPut("role")]
    [Authorize(Roles = "Admin")] // Chỉ Admin mới được cập nhật UserRole
    public async Task<IActionResult> UpdateUserRole([FromBody] UpdateUserRoleRequest request)
    {
        var response = await _authService.UpdateUserRole(request);
        return StatusCode(response.Code, response);
    }

    [HttpPut("status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUserStatus([FromBody] UpdateUserStatusRequest request)
    {
        var response = await _authService.UpdateUserStatus(request);
        return StatusCode(response.Code, response);
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteUser([FromBody] UserDeleteRequest request)
    {
        var response = await _authService.DeleteUser(request);
        return StatusCode(response.Code, response);
    }

    [HttpPost("generate")]
    [AllowAnonymous]
    public async Task<IActionResult> GenerateOtp([FromBody] GenerateOtpRequest request)
    {
        var response = await _authService.GenerateOtpAsync(request);
        return StatusCode(response.Code, response);
    }

    [HttpPut("verify")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
    {
        var response = await _authService.VerifyOtpAsync(request);
        return StatusCode(response.Code, response);
    }
}