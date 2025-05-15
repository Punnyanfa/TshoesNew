using FCSP.DTOs;
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

        [HttpGet("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetServiceById(long id)
        {
            var request = new GetServiceByIdRequest { Id = id };
            var response = await _serviceService.GetServiceById(request);
            return StatusCode(response.Code, response);
        }

        [HttpGet("manufacturer/{manufacturerId:long}")] // New endpoint
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetServicesByManufacturerId(long manufacturerId)
        {
            var response = await _serviceService.GetServicesByManufacturerId(manufacturerId);
            return StatusCode(response.Code, response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddService([FromBody] AddServiceRequest request)
        {
            var response = await _serviceService.AddService(request);
            return StatusCode(response.Code, response);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateService([FromBody] UpdateServiceRequest request)
        {
            var response = await _serviceService.UpdateService(request);
            return StatusCode(response.Code, response);
        }

        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteService(long id)
        {
            if (id <= 0)
            {
                return BadRequest(new BaseResponseModel<object>
                {
                    Code = 400,
                    Message = "Service ID must be greater than 0"
                });
            }

            var request = new DeleteServiceRequest { Id = id };
            var response = await _serviceService.DeleteService(request);
            return StatusCode(response.Code, response);
        }
    }
}