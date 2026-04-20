using AutoDealerPro.Modules.Leads.Application.Requests;
using AutoDealerPro.Modules.Leads.Application.Response;

namespace AutoDealerPro.Modules.Leads.Application.Interfaces;

public interface ILeadsService
{
    Task<LeadDetailResponse> CreateLeadAsync(CreateLeadRequest request);
    Task<LeadDetailResponse> GetLeadByIdAsync(Guid id);
    Task<IEnumerable<LeadListResponse>> GetAllLeadsAsync(int page = 1, int pageSize = 10);
    Task<IEnumerable<LeadListResponse>> GetLeadsByStatusAsync(string status, int page = 1, int pageSize = 10);
    Task<IEnumerable<LeadListResponse>> GetLeadsByTypeAsync(string type, int page = 1, int pageSize = 10);
    Task<IEnumerable<LeadListResponse>> GetLeadsAssignedToStaffAsync(Guid staffId, int page = 1, int pageSize = 10);
    Task<IEnumerable<LeadListResponse>> GetLeadsByVehicleIdAsync(Guid vehicleId);
    Task<IEnumerable<LeadListResponse>> GetPendingFollowUpsAsync(int page = 1, int pageSize = 10);
    Task AssignLeadToStaffAsync(Guid leadId, AssignLeadRequest request);
    Task MarkLeadAsContactedAsync(Guid leadId, MarkAsContactedRequest request);
    Task AddFollowUpAsync(Guid leadId, AddFollowUpRequest request);
    Task CloseLeadAsync(Guid leadId, CloseLeadRequest request);
}
