using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using FCSP.DTOs.Authentication;
using FCSP.Services.User;
using FCSP.Services.Authentication;

namespace FCSP.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService authService;

    public AuthController(IAuthService authService)
    {
        this.authService = authService;
    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult Login([FromBody] UserLoginRequest request)
    {
        var response = authService.Login(request);
        return Ok(response);
    }

    [HttpGet("test")]
    [Authorize]
    public IActionResult TestLogin()
    {
        return Ok("Authentication OK!");
    }
}
