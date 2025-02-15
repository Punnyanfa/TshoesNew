using FCSP.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace FCSP.WebAPI.Controllers.Authorization;

[Route("api/[controller]")]
[ApiController]
public class GoogleAuthController : ControllerBase
{
    private readonly IGoogleAuthService _googleAuthService;

    public GoogleAuthController(IGoogleAuthService googleAuthService)
    {
        _googleAuthService = googleAuthService;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> GoogleLogin([FromBody] string token)
    {
        return Ok(await _googleAuthService.GoogleLogin(token));
    }
}
