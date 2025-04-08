using FCSP.DTOs.Service;
using FCSP.Services.ServiceService;
using Microsoft.AspNetCore.Mvc;

namespace FCSP.WebAPI.Controllers.Service
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllServices()
        {
            var response = await _serviceService.GetAllServices();
            return StatusCode(response.Code, response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetServiceById([FromRoute] GetServiceByIdRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _serviceService.GetServiceById(request);
            return StatusCode(response.Code, response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddService([FromBody] AddServiceRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _serviceService.AddService(request);
            return StatusCode(response.Code, response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateService(long id, [FromBody] UpdateServiceRequest request)
        {
            if (id != request.Id)
                return BadRequest(new { message = "ID mismatch between route and request body" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _serviceService.UpdateService(request);
            return StatusCode(response.Code, response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteService([FromRoute] DeleteServiceRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _serviceService.DeleteService(request);
            return StatusCode(response.Code, response);
        }
    }
}