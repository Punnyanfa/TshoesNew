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
            var designServices = await _designServiceService.GetAllDesignServices();
            return Ok(designServices);
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
            try
            {
                var request = new GetDesignServiceByIdRequest { Id = id };
                var response = await _designServiceService.GetDesignServiceById(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
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
            var designServices = await _designServiceService.GetDesignServicesByCustomShoeDesignId(customShoeDesignId);
            return Ok(designServices);
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
            var designServices = await _designServiceService.GetDesignServicesByServiceId(serviceId);
            return Ok(designServices);
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
            try
            {
                var response = await _designServiceService.AddDesignService(request);
                return CreatedAtAction(nameof(GetDesignServiceById), new { id = response.DesignServiceId }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
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
            if (id != request.Id)
                return BadRequest(new { message = "ID mismatch between route and request body" });

            try
            {
                var response = await _designServiceService.UpdateDesignService(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
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
            try
            {
                var request = new DeleteDesignServiceRequest { Id = id };
                var response = await _designServiceService.DeleteDesignService(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}