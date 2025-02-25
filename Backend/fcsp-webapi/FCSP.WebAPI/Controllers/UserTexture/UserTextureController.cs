using FCSP.DTOs.UserTexture;
using FCSP.Services.UserTextureService;
using Microsoft.AspNetCore.Mvc;

namespace FCSP.WebAPI.Controllers.UserTexture
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTextureController : ControllerBase
    {
        private readonly IUserTextureService _userTextureService;

        public UserTextureController(IUserTextureService userTextureService)
        {
            _userTextureService = userTextureService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserTextures()
        {
            var result = await _userTextureService.GetAllUserTextures();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserTextureById(int id)
        {
            var request = new GetUserTextureByIdRequest { Id = id };
            var result = await _userTextureService.GetUserTextureById(request);
            return Ok(result);
        }

        [HttpGet("owner/{ownerId}")]
        public async Task<IActionResult> GetUserTexturesByOwner(int ownerId)
        {
            var request = new GetUserTexturesByOwnerRequest { OwnerId = ownerId };
            var result = await _userTextureService.GetUserTexturesByOwner(request);
            return Ok(result);
        }

        [HttpGet("buyer/{buyerId}")]
        public async Task<IActionResult> GetUserTexturesByBuyer(int buyerId)
        {
            var request = new GetUserTexturesByBuyerRequest { BuyerId = buyerId };
            var result = await _userTextureService.GetUserTexturesByBuyer(request);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserTexture([FromBody] AddUserTextureRequest request)
        {
            var result = await _userTextureService.AddUserTexture(request);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserTexture([FromBody] UpdateUserTextureRequest request)
        {
            var result = await _userTextureService.UpdateUserTexture(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserTexture(int id)
        {
            var request = new DeleteUserTextureRequest { Id = id };
            var result = await _userTextureService.DeleteUserTexture(request);
            return Ok(result);
        }
    }
} 