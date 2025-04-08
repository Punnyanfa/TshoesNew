using FCSP.DTOs.DesignService;
using FCSP.Services.DesignServiceService;
using Microsoft.AspNetCore.Mvc;

namespace FCSP.WebAPI.Controllers.DesignService
{
    /// <summary>
    /// API Controller for managing design services
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DesignServiceController : ControllerBase
    {
        private readonly IDesignServiceService _designServiceService;

        public DesignServiceController(IDesignServiceService designServiceService)
        {
            _designServiceService = designServiceService;
        }

        /// <summary>
        /// Get all design services
        /// </summary>
        /// <returns>List of all design services</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllDesignServices()
        {
            var result = await _designServiceService.GetAllDesignServices();
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Get a design service by ID
        /// </summary>
        /// <param name="id">Design service ID</param>
        /// <returns>Design service details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDesignServiceById(long id)
        {
            var request = new GetDesignServiceByIdRequest { Id = id };
            var result = await _designServiceService.GetDesignServiceById(request);
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Get design services by custom shoe design ID
        /// </summary>
        /// <param name="customShoeDesignId">Custom shoe design ID</param>
        /// <returns>List of design services</returns>
        [HttpGet("customShoeDesign/{customShoeDesignId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDesignServicesByCustomShoeDesignId(long customShoeDesignId)
        {
            var request = new GetDesignServicesByCustomShoeDesignIdRequest { CustomShoeDesignId = customShoeDesignId };
            var result = await _designServiceService.GetDesignServicesByCustomShoeDesignId(request);
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Get design services by service ID
        /// </summary>
        /// <param name="serviceId">Service ID</param>
        /// <returns>List of design services</returns>
        [HttpGet("service/{serviceId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDesignServicesByServiceId(long serviceId)
        {
            var request = new GetDesignServicesByServiceIdRequest { ServiceId = serviceId };
            var result = await _designServiceService.GetDesignServicesByServiceId(request);
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Add a new design service
        /// </summary>
        /// <param name="request">Design service data</param>
        /// <returns>Created design service</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddDesignService([FromBody] AddDesignServiceRequest request)
        {
            var result = await _designServiceService.AddDesignService(request);
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Update an existing design service
        /// </summary>
        /// <param name="id">Design service ID</param>
        /// <param name="request">Updated design service data</param>
        /// <returns>Updated design service</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateDesignService(long id, [FromBody] UpdateDesignServiceRequest request)
        {
            request.Id = id; // Gán Id từ route vào request
            var result = await _designServiceService.UpdateDesignService(request);
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Delete a design service
        /// </summary>
        /// <param name="id">Design service ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDesignService(long id)
        {
            var request = new DeleteDesignServiceRequest { Id = id };
            var result = await _designServiceService.DeleteDesignService(request);
            return StatusCode(result.Code, result);
        }
    }
}