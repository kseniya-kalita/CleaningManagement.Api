using CleaningManagement.Abstractions.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleaningManagement.Abstractions.Interfaces
{
    public interface ICleaningManagementService
    {
        public Task<CleaningPlan> CreateAsync(CleaningPlanForManipulationDto cleaningPlanDto);
        public Task<List<CleaningPlan>> GetByCustomerIdAsync(int customerId);
        public Task<CleaningPlan> UpdateByIdAsync(Guid id, CleaningPlanForManipulationDto cleaningPlanDto);
        public Task<CleaningPlan> DeleteByIdAsync(Guid id);
    }
}
