using CleaningManagement.Abstractions.Interfaces;
using CleaningManagement.Abstractions.Dtos;
using CleaningManagement.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleaningManagement.Services
{
    public class CleaningManagementService : ICleaningManagementService
    {
        private readonly ICleaningPlanRepository _repository;
        public CleaningManagementService(ICleaningPlanRepository repository)
        {
            _repository = repository;
        }

        public async Task<CleaningPlan> CreateAsync(CleaningPlanForManipulationDto cleaningPlanDto)
        {
            CleaningPlan cleaningPlan = new CleaningPlan
            {
                CreationDate = DateTime.UtcNow,
                CustomerId = cleaningPlanDto.CustomerId,
                Description = cleaningPlanDto.Description,
                Title = cleaningPlanDto.Title
            };
            await _repository.CreateAsync(cleaningPlan).ConfigureAwait(false);
            
            return cleaningPlan;
        }

        public async Task<List<CleaningPlan>> GetByCustomerIdAsync(int customerId)
        {
            var plans = await _repository.GetByCustomerIdAsync(customerId).ConfigureAwait(false);

            if (plans.Count == 0)
            {
                return null;
            }
            return plans;
        }

        public async Task<CleaningPlan> UpdateByIdAsync(Guid id, CleaningPlanForManipulationDto cleaningPlanDto)
        {
            var cleaningPlan = await _repository.GetByIdAsync(id).ConfigureAwait(false);
            if (cleaningPlan == null)
            {
                return null;
            }

            cleaningPlan.Title = cleaningPlanDto.Title;
            cleaningPlan.Description = cleaningPlan.Description;
            cleaningPlan.CustomerId = cleaningPlan.CustomerId;

            await _repository.UpdateAsync(cleaningPlan).ConfigureAwait(false);
            return cleaningPlan;
        }

        public async Task<CleaningPlan> DeleteByIdAsync(Guid id)
        {
            var cleaningPlan = await _repository.GetByIdAsync(id).ConfigureAwait(false);
            if (cleaningPlan == null)
            {
                return null;
            }

            await _repository.DeleteAsync(cleaningPlan).ConfigureAwait(false);
            return cleaningPlan;
        }



    }
}
