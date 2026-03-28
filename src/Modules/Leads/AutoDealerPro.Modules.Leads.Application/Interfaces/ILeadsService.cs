using AutoDealerPro.Modules.Leads.Application.DTOs;
using AutoDealerPro.Modules.Leads.Application.Requests;

namespace AutoDealerPro.Modules.Leads.Application.Interfaces;

public interface ILeadsService
{
    Task<LeadDetailDto> CreateLeadAsync(CreateLeadRequest request);
    Task<LeadDetailDto> GetLeadByIdAsync(Guid id);
    Task<IEnumerable<LeadListDto>> GetAllLeadsAsync(int page = 1, int pageSize = 10);
    Task<IEnumerable<LeadListDto>> GetLeadsByStatusAsync(string status, int page = 1, int pageSize = 10);
    Task<IEnumerable<LeadListDto>> GetLeadsByTypeAsync(string type, int page = 1, int pageSize = 10);
    Task<IEnumerable<LeadListDto>> GetLeadsAssignedToStaffAsync(Guid staffId, int page = 1, int pageSize = 10);
    Task<IEnumerable<LeadListDto>> GetLeadsByVehicleIdAsync(Guid vehicleId);
    Task<IEnumerable<LeadListDto>> GetPendingFollowUpsAsync(int page = 1, int pageSize = 10);
    Task AssignLeadToStaffAsync(Guid leadId, AssignLeadRequest request);
    Task MarkLeadAsContactedAsync(Guid leadId, MarkAsContactedRequest request);
    Task AddFollowUpAsync(Guid leadId, AddFollowUpRequest request);
    Task CloseLeadAsync(Guid leadId, CloseLeadRequest request);
}
