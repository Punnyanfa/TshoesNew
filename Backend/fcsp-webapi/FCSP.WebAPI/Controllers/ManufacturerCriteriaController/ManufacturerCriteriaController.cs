using FCSP.DTOs;
using FCSP.DTOs.Criteria;
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
        /// Adds a new criteria.
        /// </summary>
        /// <param name="request">The criteria details.</param>
        /// <returns>The added criteria details.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponseModel<AddCriteriaResponse>>> AddCriteria([FromBody] AddCriteriaRequest request)
        {
            var response = await _manufacturerCriteriaService.AddCriteriaAsync(request);
            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Gets a criteria by ID.
        /// </summary>
        /// <param name="id">The criteria ID.</param>
        /// <returns>The criteria details.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponseModel<GetCriteriaResponse>>> GetCriteria(long id)
        {
            var request = new GetCriteriaRequest { Id = id };
            var response = await _manufacturerCriteriaService.GetCriteriaAsync(request);
            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Updates an existing criteria.
        /// </summary>
        /// <param name="request">The updated criteria details.</param>
        /// <returns>The updated criteria details.</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponseModel<UpdateCriteriaResponse>>> UpdateCriteria([FromBody] UpdateCriteriaRequest request)
        {
            var response = await _manufacturerCriteriaService.UpdateCriteriaAsync(request);
            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Deletes a criteria by ID.
        /// </summary>
        /// <param name="id">The criteria ID.</param>
        /// <returns>Whether the deletion was successful.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponseModel<bool>>> DeleteCriteria(long id)
        {
            var response = await _manufacturerCriteriaService.DeleteCriteriaAsync(id);
            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Gets all active criteria.
        /// </summary>
        /// <returns>A list of active criteria.</returns>
        [HttpGet("active")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponseModel<List<GetCriteriaResponse>>>> GetAllActiveCriteria()
        {
            var response = await _manufacturerCriteriaService.GetAllActiveCriteriaAsync();
            return StatusCode(response.Code, response);
        }
    }
}