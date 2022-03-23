using CleaningManagement.Abstractions.Interfaces;
using CleaningManagement.Abstractions.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CleaningManagement.Api.Controllers
{
    [ApiController]
    [Route("api/cleaningplans")]
    public class CleaningPlansController : ControllerBase
    {
        private readonly ICleaningManagementService _service;
        public CleaningPlansController(ICleaningManagementService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCleaningPlan([FromBody]CleaningPlanForManipulationDto cleaningPlanDto)
        {
            if (cleaningPlanDto == null)
            {
                return BadRequest("Cleaning plan model cannot be null");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cleaningPlan = await _service.CreateAsync(cleaningPlanDto).ConfigureAwait(false);
            
            return new ObjectResult(cleaningPlan) { StatusCode = StatusCodes.Status201Created };
        }

        [HttpGet]
        public async Task<IActionResult> GetCleaningPlansByCustomerId(int customerId)
        {
            var plans = await _service.GetByCustomerIdAsync(customerId).ConfigureAwait(false);
            if (plans == null)
            {
                return NotFound();
            }

            return Ok(plans);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCleaningPlanById(Guid id, [FromBody] CleaningPlanForManipulationDto cleaningPlanDto)
        {
            if (cleaningPlanDto == null || !ModelState.IsValid)
            {
                return BadRequest();
            }
            var updatedPlan = await _service.UpdateByIdAsync(id, cleaningPlanDto).ConfigureAwait(false);
            
            if (updatedPlan == null)
            {
                return NotFound();
            }

            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCleaningPlanById(Guid id)
        {
            var deletedPlan = await _service.DeleteByIdAsync(id).ConfigureAwait(false);
            if (deletedPlan == null)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}