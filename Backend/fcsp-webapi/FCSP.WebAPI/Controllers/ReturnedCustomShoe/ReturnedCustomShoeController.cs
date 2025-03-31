using FCSP.DTOs;
using FCSP.DTOs.ReturnedCustomShoe;
using FCSP.Services.ReturnedCustomShoeService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FCSP.WebAPI.Controllers.ReturnedCustomShoe
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReturnedCustomShoeController : ControllerBase
    {
        private readonly IReturnedCustomShoeService _returnedCustomShoeService;

        public ReturnedCustomShoeController(IReturnedCustomShoeService returnedCustomShoeService)
        {
            _returnedCustomShoeService = returnedCustomShoeService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BaseResponseModel<AddReturnedCustomShoeResponse>>> AddReturnedCustomShoe(AddReturnedCustomShoeRequest request)
        {
            var response = await _returnedCustomShoeService.AddReturnedCustomShoe(request);
            return Ok(response);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BaseResponseModel<GetReturnedCustomShoesResponse>>> GetAllReturnedCustomShoes()
        {
            var response = await _returnedCustomShoeService.GetAllReturnedCustomShoes();
            return Ok(response);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BaseResponseModel<GetReturnedCustomShoeByIdResponse>>> GetReturnedCustomShoeById(long id)
        {
            var request = new GetReturnedCustomShoeByIdRequest { Id = id };
            var response = await _returnedCustomShoeService.GetReturnedCustomShoeById(request);
            return Ok(response);
        }

        [HttpGet("design/{designId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BaseResponseModel<GetReturnedCustomShoesResponse>>> GetReturnedCustomShoesByDesignId(long designId)
        {
            var response = await _returnedCustomShoeService.GetReturnedCustomShoesByDesignId(designId);
            return Ok(response);
        }
    }
} 