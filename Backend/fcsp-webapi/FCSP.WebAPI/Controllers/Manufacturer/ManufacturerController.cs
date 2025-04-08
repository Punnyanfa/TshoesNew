using FCSP.DTOs;
using FCSP.DTOs.Manufacturer;
using FCSP.Models.Entities;
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
        private readonly ILogger<ManufacturerController> _logger;

        public ManufacturerController(IManufacturerService manufacturerService, ILogger<ManufacturerController> logger)
        {
            _manufacturerService = manufacturerService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllManufacturers()
        {
            _logger.LogInformation("Retrieving all manufacturers");
            var response = await _manufacturerService.GetAllManufacturers();
            return StatusCode(response.Code, response);
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetManufacturerById(long id)
        {
            _logger.LogInformation("Retrieving manufacturer with ID: {Id}", id);
            var request = new GetManufacturerRequest { Id = id };
            if (id <= 0)
            {
                _logger.LogWarning("Invalid ID provided: {Id}", id);
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
            _logger.LogInformation("Adding new manufacturer for UserId: {UserId}", request.UserId);
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                _logger.LogWarning("Validation failed: {Errors}", string.Join(", ", errors));
                return BadRequest(new BaseResponseModel<object> { Code = 400, Message = "Validation failed", Data = errors });
            }

            var response = await _manufacturerService.AddManufacturer(request);
            if (response.Code == 201)
            {
                _logger.LogInformation("Manufacturer added successfully with ID: {Id}", response.Data.Id);
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
            _logger.LogInformation("Updating manufacturer with ID: {Id}", id);
            var validationResult = ValidateRequest(id, request, request.Id);
            if (validationResult != null) return validationResult;

            var response = await _manufacturerService.UpdateManufacturer(request);
            return StatusCode(response.Code, response);
        }

        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteManufacturer(long id)
        {
            _logger.LogInformation("Deleting manufacturer with ID: {Id}", id);
            var request = new GetManufacturerRequest { Id = id };
            if (id <= 0)
            {
                _logger.LogWarning("Invalid ID provided: {Id}", id);
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
            _logger.LogInformation("Retrieving manufacturers for UserId: {UserId}", userId);
            if (userId <= 0)
            {
                _logger.LogWarning("Invalid UserId provided: {UserId}", userId);
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
            _logger.LogInformation("Retrieving all active manufacturers");
            var response = await _manufacturerService.GetActiveManufacturers();
            return StatusCode(response.Code, response);
        }

        private IActionResult ValidateRequest<T>(long routeId, T request, long requestId)
        {
            if (routeId != requestId)
                return BadRequest(new BaseResponseModel<object> { Code = 400, Message = "ID mismatch between route and request body" });
            if (!ModelState.IsValid)
                return BadRequest(new BaseResponseModel<object> { Code = 400, Message = "Validation failed", Data = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            return null;
        }
    }
}