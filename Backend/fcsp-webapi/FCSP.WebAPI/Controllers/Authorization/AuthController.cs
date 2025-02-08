using FCSP.DTOs.Authentication;
using FCSP.Services.Authentication;
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
    public IActionResult Login([FromBody] UserLoginRequest request)
    {
        var response = _authService.Login(request);
        return Ok(response);
    }

    [HttpPost("[action]")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
    {
        var response = await _authService.Register(request);
        return Ok(response);
    }

    [HttpGet("test")]
    [Authorize]
    public IActionResult TestLogin()
    {
        return Ok("Authentication OK!");
    }
}
