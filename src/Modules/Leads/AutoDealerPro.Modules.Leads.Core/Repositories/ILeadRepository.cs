using AutoDealerPro.Modules.Leads.Core.Entities;
using AutoDealerPro.Modules.Leads.Core.Enums;

namespace AutoDealerPro.Modules.Leads.Core.Repositories
{
    public interface ILeadRepository
    {
        Task<Lead?> GetByIdAsync(Guid id);
        Task<Lead?> GetByEmailAsync(string email);
        Task<IEnumerable<Lead>> GetByStatusAsync(LeadStatus status, int page = 1, int pageSize = 10);
        Task<IEnumerable<Lead>> GetByTypeAsync(LeadType type, int page = 1, int pageSize = 10);
        Task<IEnumerable<Lead>> GetAssignedToStaffAsync(Guid staffId, int page = 1, int pageSize = 10);
        Task<IEnumerable<Lead>> GetByVehicleIdAsync(Guid vehicleId);
        Task<IEnumerable<Lead>> GetPendingFollowUpsAsync(int page = 1, int pageSize = 10);
        Task<IEnumerable<Lead>> GetAllAsync(int page = 1, int pageSize = 10);
        Task<Lead> AddAsync(Lead lead);
        Task UpdateAsync(Lead lead);
        Task DeleteAsync(Guid id);
        Task<int> GetCountByStatusAsync(LeadStatus status);
        Task<int> GetCountAsync();
    }
}
