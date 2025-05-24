using FCSP.DTOs;
using FCSP.DTOs.Manufacturer;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using FCSP.Services.ManufacturerService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FCSP.WebAPI.Controllers.Manufacturer
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManufacturerController : ControllerBase
    {
        private readonly IManufacturerService _manufacturerService;

        public ManufacturerController(IManufacturerService manufacturerService)
        {
            _manufacturerService = manufacturerService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllManufacturers()
        {
            var response = await _manufacturerService.GetAllManufacturers();
            return StatusCode(response.Code, response);
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetManufacturerById(long id)
        {
            var request = new GetManufacturerRequest { Id = id };
            if (id <= 0)
            {
                return BadRequest(new BaseResponseModel<object>
                {
                    Code = 400,
                    Message = "Manufacturer ID must be greater than 0"
                });
            }

            var response = await _manufacturerService.GetManufacturerById(request);
            return StatusCode(response.Code, response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddManufacturer([FromBody] AddManufacturerRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new BaseResponseModel<object> { Code = 400, Message = "Validation failed", Data = errors });
            }

            var response = await _manufacturerService.AddManufacturer(request);
            if (response.Code == 201)
            {
                return CreatedAtAction(nameof(GetManufacturerById), new { id = response.Data.Id }, response);
            }
            return StatusCode(response.Code, response);
        }

        [HttpPut("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateManufacturer(long id, [FromBody] UpdateManufacturerRequest request)
        {           
            var response = await _manufacturerService.UpdateManufacturer(request);
            return StatusCode(response.Code, response);
        }
        [HttpPut("updateStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateManufacturerStatus([FromBody] UpdateManufacturerStatusRequest request)
        {               
            var response = await _manufacturerService.UpdateManufacturerStatus(request);
            return StatusCode(response.Code, response);
        }

        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteManufacturer(long id)
        {
            var request = new GetManufacturerRequest { Id = id };
            if (id <= 0)
            {
                return BadRequest(new BaseResponseModel<object>
                {
                    Code = 400,
                    Message = "Manufacturer ID must be greater than 0"
                });
            }

            var response = await _manufacturerService.DeleteManufacturer(request);
            return StatusCode(response.Code, response);
        }

        [HttpGet("user/{userId:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetManufacturersByUserId(long userId)
        {
            if (userId <= 0)
            {
                return BadRequest(new BaseResponseModel<object> { Code = 400, Message = "User ID must be greater than 0" });
            }

            var response = await _manufacturerService.GetManufacturersByUserId(userId);
            return StatusCode(response.Code, response);
        }

        [HttpGet("active")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetActiveManufacturers()
        {
        var response = await _manufacturerService.GetActiveManufacturers();
            return StatusCode(response.Code, response);
        }  
    }
}