using CleaningManagement.Api.Boundary.Request;
using CleaningManagement.DAL;
using CleaningManagement.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CleaningManagement.Api.Controllers
{
    [ApiController]
    [Route("api/cleaningplans")]
    public class CleaningPlansController : ControllerBase
    {
        private readonly ICleaningPlanRepository _repository;
        public CleaningPlansController(ICleaningPlanRepository repository)
        {
            _repository = repository;
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

            CleaningPlan cleaningPlan = new CleaningPlan
            {
                CreationDate = DateTime.UtcNow,
                CustomerId = cleaningPlanDto.CustomerId,
                Description = cleaningPlanDto.Description,
                Title = cleaningPlanDto.Title
            };
            await _repository.CreateAsync(cleaningPlan).ConfigureAwait(false);

            return Ok(cleaningPlan);
        }

        [HttpGet]
        public async Task<IActionResult> GetCleaningPlansByCustomerId(int customerId)
        {
            var plans = await _repository.GetByCustomerIdAsync(customerId).ConfigureAwait(false);
            
            if (plans.Count == 0)
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
            
            var cleaningPlan = await _repository.GetByIdAsync(id).ConfigureAwait(false);
            if (cleaningPlan == null)
            {
                return NotFound();
            }

            cleaningPlan.Title = cleaningPlanDto.Title;
            cleaningPlan.Description = cleaningPlan.Description;
            cleaningPlan.CustomerId = cleaningPlan.CustomerId;
            
            await _repository.UpdateAsync(cleaningPlan).ConfigureAwait(false);

            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCleaningPlanById(Guid id)
        {
            var cleaningPlan = await _repository.GetByIdAsync(id).ConfigureAwait(false);
            if (cleaningPlan == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(cleaningPlan).ConfigureAwait(false);

            return NoContent();
        }

    }
}