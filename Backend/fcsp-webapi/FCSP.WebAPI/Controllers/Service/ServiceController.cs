using FCSP.DTOs.Service;
using FCSP.Services.ServiceService;
using Microsoft.AspNetCore.Mvc;

namespace FCSP.WebAPI.Controllers.Service
{
    /// <summary>
    /// API Controller for managing services
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        /// <summary>
        /// Get all services
        /// </summary>
        /// <returns>List of all services</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllServices()
        {
            var services = await _serviceService.GetAllServices();
            return Ok(services);
        }

        /// <summary>
        /// Get a service by ID
        /// </summary>
        /// <param name="id">Service ID</param>
        /// <returns>Service details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetServiceById(long id)
        {
            try
            {
                var request = new GetServiceByIdRequest { Id = id };
                var response = await _serviceService.GetServiceById(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Add a new service
        /// </summary>
        /// <param name="request">Service data</param>
        /// <returns>Created service</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddService([FromBody] AddServiceRequest request)
        {
            try
            {
                var response = await _serviceService.AddService(request);
                return CreatedAtAction(nameof(GetServiceById), new { id = response.ServiceId }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing service
        /// </summary>
        /// <param name="id">Service ID</param>
        /// <param name="request">Updated service data</param>
        /// <returns>Updated service</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateService(long id, [FromBody] UpdateServiceRequest request)
        {
            if (id != request.Id)
                return BadRequest(new { message = "ID mismatch between route and request body" });

            try
            {
                var response = await _serviceService.UpdateService(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete a service (soft delete)
        /// </summary>
        /// <param name="id">Service ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteService(long id)
        {
            try
            {
                var request = new DeleteServiceRequest { Id = id };
                var response = await _serviceService.DeleteService(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}