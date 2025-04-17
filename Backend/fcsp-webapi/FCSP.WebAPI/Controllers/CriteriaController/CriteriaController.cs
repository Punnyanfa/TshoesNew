using FCSP.DTOs;
using FCSP.DTOs.Criteria;
using FCSP.Services.CriteriaService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FCSP.WebAPI.Controllers.CriteriaController
{
    [Route("api/[controller]")]
    [ApiController]
    public class CriteriaController : ControllerBase
    {
        private readonly ICriteriaService _criteriaService;

        public CriteriaController(ICriteriaService criteriaService)
        {
            _criteriaService = criteriaService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddCriteria([FromBody] AddCriteriaRequest request)
        {
            var response = await _criteriaService.AddAsync(request);
            return StatusCode(response.Code, response);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCriteria([FromBody] UpdateCriteriaRequest request)
        {
            var response = await _criteriaService.UpdateAsync(request);
            return StatusCode(response.Code, response);
        }
        [HttpPut("{id}/restore")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCriteriaStatus(long id)
        {
            var response = await _criteriaService.UpdateStatusAsync(id);
            return StatusCode(response.Code, response);
        }  

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCriteria(long id)
        {
            var response = await _criteriaService.DeleteAsync(id);
            return StatusCode(response.Code, response);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllCriteria()
        {
            var response = await _criteriaService.GetAllAsync();
            return StatusCode(response.Code, response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCriteriaById(long id)
        {
            var response = await _criteriaService.GetByIdAsync(id);
            return StatusCode(response.Code, response);
        }
    }
}