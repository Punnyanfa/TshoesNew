using FCSP.DTOs;
using FCSP.DTOs.ManufacturerCriteria;
using FCSP.Services.ManufacturerCriteriaService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FCSP.WebAPI.Controllers.ManufacturerCriteriaController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManufacturerCriteriaController : ControllerBase
    {
        private readonly IManufacturerCriteriaService _manufacturerCriteriaService;

        public ManufacturerCriteriaController(IManufacturerCriteriaService manufacturerCriteriaService)
        {
            _manufacturerCriteriaService = manufacturerCriteriaService ?? throw new ArgumentNullException(nameof(manufacturerCriteriaService));
        }

        /// <summary>
        /// Adds a new manufacturer-criteria relationship.
        /// </summary>
        /// <param name="request">The manufacturer-criteria details.</param>
        /// <returns>The added manufacturer-criteria details.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponseModel<AddManufacturerCriteriaResponse>>> AddManufacturerCriteria([FromBody] AddManufacturerCriteriaRequest request)
        {
            var response = await _manufacturerCriteriaService.AddManufacturerCriteriaAsync(request);
            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Gets a manufacturer-criteria by ID.
        /// </summary>
        /// <param name="id">The manufacturer-criteria ID.</param>
        /// <returns>The manufacturer-criteria details.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponseModel<GetManufacturerCriteriaResponse>>> GetManufacturerCriteria(long id)
        {
            var request = new GetManufacturerCriteriaRequest { Id = id };
            var response = await _manufacturerCriteriaService.GetManufacturerCriteriaAsync(request);
            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Updates an existing manufacturer-criteria.
        /// </summary>
        /// <param name="request">The updated manufacturer-criteria details.</param>
        /// <returns>The updated manufacturer-criteria details.</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponseModel<UpdateManufacturerCriteriaResponse>>> UpdateManufacturerCriteria([FromBody] UpdateManufacturerCriteriaRequest request)
        {
            var response = await _manufacturerCriteriaService.UpdateManufacturerCriteriaAsync(request);
            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Deletes a manufacturer-criteria by ID.
        /// </summary>
        /// <param name="id">The manufacturer-criteria ID.</param>
        /// <returns>Whether the deletion was successful.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponseModel<bool>>> DeleteManufacturerCriteria(long id)
        {
            var response = await _manufacturerCriteriaService.DeleteManufacturerCriteriaAsync(id);
            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Gets all manufacturer-criteria relationships.
        /// </summary>
        /// <returns>A list of manufacturer-criteria relationships.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponseModel<List<GetManufacturerCriteriaResponse>>>> GetAllManufacturerCriteria()
        {
            var response = await _manufacturerCriteriaService.GetAllManufacturerCriteriaAsync();
            return StatusCode(response.Code, response);
        }
    }
}