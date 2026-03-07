using AutoDealerPro.Modules.Leads.Core.Entities;
using AutoDealerPro.Modules.Leads.Core.Enums;
using AutoDealerPro.Modules.Leads.Core.Repositories;
using AutoDealerPro.Modules.Leads.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AutoDealerPro.Modules.Leads.Infrastructure.Repositories
{
    public class LeadRepository(LeadsDbContext context) : ILeadRepository
    {
        private readonly LeadsDbContext _context = context;

        public async Task<Lead?> GetByIdAsync(Guid id)
            => await _context.Leads
                .Include(l => l.FollowUps)
                .FirstOrDefaultAsync(l => l.Id == id);

        public async Task<Lead?> GetByEmailAsync(string email)
            => await _context.Leads
                .Include(l => l.FollowUps)
                .FirstOrDefaultAsync(l => l.Email == email.ToLowerInvariant());

        public async Task<IEnumerable<Lead>> GetByStatusAsync(LeadStatus status, int page = 1, int pageSize = 10)
        {
            var skip = (page - 1) * pageSize;
            return await _context.Leads
                .Where(l => l.Status == status)
                .Include(l => l.FollowUps)
                .OrderByDescending(l => l.CreatedAt)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Lead>> GetByTypeAsync(LeadType type, int page = 1, int pageSize = 10)
        {
            var skip = (page - 1) * pageSize;
            return await _context.Leads
                .Where(l => l.Type == type)
                .Include(l => l.FollowUps)
                .OrderByDescending(l => l.CreatedAt)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Lead>> GetAssignedToStaffAsync(Guid staffId, int page = 1, int pageSize = 10)
        {
            var skip = (page - 1) * pageSize;
            return await _context.Leads
                .Where(l => l.AssignedToStaffId == staffId)
                .Include(l => l.FollowUps)
                .OrderByDescending(l => l.CreatedAt)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Lead>> GetByVehicleIdAsync(Guid vehicleId)
            => await _context.Leads
                .Where(l => l.VehicleId == vehicleId)
                .Include(l => l.FollowUps)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();

        public async Task<IEnumerable<Lead>> GetPendingFollowUpsAsync(int page = 1, int pageSize = 10)
        {
            var skip = (page - 1) * pageSize;
            var today = DateTime.UtcNow.Date;

            return await _context.Leads
                .Where(l => l.FollowUps.Any(f => f.NextFollowUpDate <= today && f.NextFollowUpDate != null))
                .Include(l => l.FollowUps)
                .OrderByDescending(l => l.CreatedAt)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Lead>> GetAllAsync(int page = 1, int pageSize = 10)
        {
            var skip = (page - 1) * pageSize;
            return await _context.Leads
                .Include(l => l.FollowUps)
                .OrderByDescending(l => l.CreatedAt)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Lead> AddAsync(Lead lead)
        {
            await _context.Leads.AddAsync(lead);
            await _context.SaveChangesAsync();
            return lead;
        }

        public async Task UpdateAsync(Lead lead)
        {
            _context.Leads.Update(lead);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var lead = await _context.Leads.FindAsync(id);
            if (lead is not null)
            {
                _context.Leads.Remove(lead);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetCountByStatusAsync(LeadStatus status)
            => await _context.Leads.CountAsync(l => l.Status == status);

        public async Task<int> GetCountAsync()
            => await _context.Leads.CountAsync();
    }
}
