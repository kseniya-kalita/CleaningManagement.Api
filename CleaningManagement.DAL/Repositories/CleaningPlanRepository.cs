using CleaningManagement.Abstractions;
using CleaningManagement.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningManagement.DAL.Repositories
{
    public class CleaningPlanRepository : ICleaningPlanRepository
    {
        private readonly CleaningManagementDbContext _context;
        public CleaningPlanRepository(CleaningManagementDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(CleaningPlan cleaningPlan)
        {
            await _context.AddAsync(cleaningPlan).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
        
        public async Task<List<CleaningPlan>> GetByCustomerIdAsync(int customerId)
        {
            return await _context
                .CleaningPlans
                .Where(p => p.CustomerId == customerId)
                .ToListAsync().ConfigureAwait(false);

        }

        public async Task<CleaningPlan> GetByIdAsync(Guid id)
        {
            return await _context
                .CleaningPlans
                .FirstOrDefaultAsync(p => p.Id == id)
                .ConfigureAwait(false);
        }

        public async Task DeleteAsync(CleaningPlan cleaningPlan)
        {
            _context.Remove(cleaningPlan);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
        
        public async Task UpdateAsync(CleaningPlan cleaningPlan)
        {
            _context.Update(cleaningPlan);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }


    }
}
