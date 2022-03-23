using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleaningManagement.DAL.Repositories
{
    public interface ICleaningPlanRepository
    {
        public Task CreateAsync(CleaningPlan cleaningPlan);
        public Task<List<CleaningPlan>> GetByCustomerIdAsync(int customerId);
        public Task<CleaningPlan> GetByIdAsync(Guid id);
        public Task DeleteAsync(CleaningPlan cleaningPlan);
        public Task UpdateAsync(CleaningPlan cleaningPlan);
    }
}
