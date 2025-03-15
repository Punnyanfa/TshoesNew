using FCSP.DTOs;
using FCSP.DTOs.Authentication;
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

    [HttpPost("[action]")]
    [AllowAnonymous]
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

    [HttpGet("test")]
    [Authorize]
    public IActionResult TestLogin()
    {
        return Ok("Authentication OK!");
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

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteUser([FromBody] UserDeleteRequest request)
    {
        var response = await _authService.DeleteUser(request);
        return StatusCode(response.Code, response);
    }
}
