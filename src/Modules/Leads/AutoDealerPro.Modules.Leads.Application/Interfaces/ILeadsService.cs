using AutoDealerPro.Modules.Leads.Application.Requests.V1;
using AutoDealerPro.Modules.Leads.Application.Requests.V1.AddFollowUp;
using AutoDealerPro.Modules.Leads.Application.Requests.V1.AssignLead;
using AutoDealerPro.Modules.Leads.Application.Requests.V1.CreateLead;
using AutoDealerPro.Modules.Leads.Application.Requests.V1.MarkAsContacted;
using AutoDealerPro.Modules.Leads.Application.Response.V1;

namespace AutoDealerPro.Modules.Leads.Application.Interfaces;

public interface ILeadsService
{
    Task<LeadDetailResponseV1> CreateLeadAsync(CreateLeadRequestV1 request);
    Task<LeadDetailResponseV1> GetLeadByIdAsync(Guid id);
    Task<IEnumerable<LeadListResponseV1>> GetAllLeadsAsync(int page = 1, int pageSize = 10);
    Task<IEnumerable<LeadListResponseV1>> GetLeadsByStatusAsync(string status, int page = 1, int pageSize = 10);
    Task<IEnumerable<LeadListResponseV1>> GetLeadsByTypeAsync(string type, int page = 1, int pageSize = 10);
    Task<IEnumerable<LeadListResponseV1>> GetLeadsAssignedToStaffAsync(Guid staffId, int page = 1, int pageSize = 10);
    Task<IEnumerable<LeadListResponseV1>> GetLeadsByVehicleIdAsync(Guid vehicleId);
    Task<IEnumerable<LeadListResponseV1>> GetPendingFollowUpsAsync(int page = 1, int pageSize = 10);
    Task AssignLeadToStaffAsync(Guid leadId, AssignLeadRequestV1 request);
    Task MarkLeadAsContactedAsync(Guid leadId, MarkAsContactedRequestV1 request);
    Task AddFollowUpAsync(Guid leadId, AddFollowUpRequestV1 request);
    Task CloseLeadAsync(Guid leadId, CloseLeadRequestv1 request);
}
