using AutoDealerPro.Modules.Leads.Core.Entities;
using AutoDealerPro.Modules.Leads.Core.Enums;

namespace AutoDealerPro.Modules.Leads.Core.Repositories;

public interface ILeadRepository
{
    Task<Lead?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Lead?> GetByEmailAsync(string email, CancellationToken ct);
    Task<IEnumerable<Lead>> GetByStatusAsync(LeadStatus status, CancellationToken ct, int page = 1, int pageSize = 10);
    Task<IEnumerable<Lead>> GetByTypeAsync(LeadType type, CancellationToken ct, int page = 1, int pageSize = 10);
    Task<IEnumerable<Lead>> GetAssignedToStaffAsync(Guid staffId, CancellationToken ct, int page = 1, int pageSize = 10);
    Task<IEnumerable<Lead>> GetByVehicleIdAsync(Guid vehicleId, CancellationToken ct);
    Task<IEnumerable<Lead>> GetPendingFollowUpsAsync(CancellationToken ct, int page = 1, int pageSize = 10);
    Task<IEnumerable<Lead>> GetAllAsync(CancellationToken ct, int page = 1, int pageSize = 10);
    Task<Lead> AddAsync(Lead lead, CancellationToken ct);
    Task UpdateAsync(Lead lead, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
    Task<int> GetCountByStatusAsync(LeadStatus status, CancellationToken ct);
    Task<int> GetCountAsync(CancellationToken ct);
}
