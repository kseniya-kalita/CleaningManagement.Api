using CleaningManagement.Abstractions.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleaningManagement.Abstractions.Interfaces
{
    public interface ICleaningManagementService
    {
        /// <summary>
        /// Create cleaning plan by dto model.
        /// </summary>
        /// <returns>Created cleaning plan.</returns>
        public Task<CleaningPlan> CreateAsync(CleaningPlanForManipulationDto cleaningPlanDto);

        /// <summary>
        /// Get list of cleaning plans by their customer id.
        /// </summary>
        /// <returns>Cleaning plans with provided customer id if found, else null.</returns>
        public Task<List<CleaningPlan>> GetByCustomerIdAsync(int customerId);

        /// <summary>
        /// Update the cleaning plan with specified id.
        /// </summary>
        /// <returns>Updated cleaning plan if found, else null.</returns>
        public Task<CleaningPlan> UpdateByIdAsync(Guid id, CleaningPlanForManipulationDto cleaningPlanDto);

        /// <summary>
        /// Delete the cleaning plan by specified id.
        /// </summary>
        /// <returns>Deleted cleaning plan if found, else null.</returns>
        public Task<CleaningPlan> DeleteByIdAsync(Guid id);
    }
}
