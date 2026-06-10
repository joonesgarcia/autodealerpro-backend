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

        public async Task<Lead?> GetByIdAsync(Guid id, CancellationToken ct)
            => await _context.Leads
                .Include(l => l.FollowUps)
                .FirstOrDefaultAsync(l => l.Id == id, cancellationToken: ct);

        public async Task<Lead?> GetByEmailAsync(string email, CancellationToken ct)
            => await _context.Leads
                .Include(l => l.FollowUps)
                .FirstOrDefaultAsync(l => l.Email == email.ToLowerInvariant(), cancellationToken: ct);

        public async Task<IEnumerable<Lead>> GetByStatusAsync(LeadStatus status, CancellationToken ct, int page = 1, int pageSize = 10)
        {
            int skip = (page - 1) * pageSize;
            return await _context.Leads
                .Where(l => l.Status == status)
                .Include(l => l.FollowUps)
                .OrderByDescending(l => l.CreatedAt)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<Lead>> GetByTypeAsync(LeadType type, CancellationToken ct, int page = 1, int pageSize = 10)
        {
            int skip = (page - 1) * pageSize;
            return await _context.Leads
                .Where(l => l.Type == type)
                .Include(l => l.FollowUps)
                .OrderByDescending(l => l.CreatedAt)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<Lead>> GetAssignedToStaffAsync(Guid staffId, CancellationToken ct, int page = 1, int pageSize = 10)
        {
            int skip = (page - 1) * pageSize;
            return await _context.Leads
                .Where(l => l.AssignedToStaffId == staffId)
                .Include(l => l.FollowUps)
                .OrderByDescending(l => l.CreatedAt)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<Lead>> GetByVehicleIdAsync(Guid vehicleId, CancellationToken ct)
            => await _context.Leads
                .Where(l => l.VehicleId == vehicleId)
                .Include(l => l.FollowUps)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync(ct);

        public async Task<IEnumerable<Lead>> GetPendingFollowUpsAsync(CancellationToken ct, int page = 1, int pageSize = 10)
        {
            int skip = (page - 1) * pageSize;
            DateTime today = DateTime.UtcNow.Date;

            return await _context.Leads
                .Where(l => l.FollowUps.Any(f => f.NextFollowUpDate <= today && f.NextFollowUpDate != null))
                .Include(l => l.FollowUps)
                .OrderByDescending(l => l.CreatedAt)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<Lead>> GetAllAsync(CancellationToken ct, int page = 1, int pageSize = 10)
        {
            int skip = (page - 1) * pageSize;
            return await _context.Leads
                .Include(l => l.FollowUps)
                .OrderByDescending(l => l.CreatedAt)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public async Task<Lead> AddAsync(Lead lead, CancellationToken ct)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                await _context.Leads.AddAsync(lead, ct);
                await _context.SaveChangesAsync(ct);

                await transaction.CommitAsync(ct);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(ct);
                throw;
            }

            return lead;
        }

        public async Task UpdateAsync(Lead lead, CancellationToken ct)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                await _context.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);

            }
            catch (Exception)
            {
                await transaction.RollbackAsync(ct);
                throw;
            }

        }

        public async Task DeleteAsync(Guid id, CancellationToken ct)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                Lead? lead = await _context.Leads.FindAsync([id], cancellationToken: ct);
                if (lead is not null)
                {
                    _context.Leads.Remove(lead);
                    await _context.SaveChangesAsync(ct);
                }
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(ct);
                throw;
            }

        }

        public async Task<int> GetCountByStatusAsync(LeadStatus status, CancellationToken ct)
            => await _context.Leads.CountAsync(l => l.Status == status, ct);

        public async Task<int> GetCountAsync(CancellationToken ct)
            => await _context.Leads.CountAsync(ct);
    }
}
