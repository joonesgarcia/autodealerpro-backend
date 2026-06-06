using AutoDealerPro.Modules.Leads.Application.Exceptions;
using AutoDealerPro.Modules.Leads.Application.Extensions.V1;
using AutoDealerPro.Modules.Leads.Application.Interfaces;
using AutoDealerPro.Modules.Leads.Application.Requests.V1;
using AutoDealerPro.Modules.Leads.Application.Requests.V1.AddFollowUp;
using AutoDealerPro.Modules.Leads.Application.Requests.V1.AssignLead;
using AutoDealerPro.Modules.Leads.Application.Requests.V1.CreateLead;
using AutoDealerPro.Modules.Leads.Application.Requests.V1.MarkAsContacted;
using AutoDealerPro.Modules.Leads.Application.Response.V1;
using AutoDealerPro.Modules.Leads.Core.Entities;
using AutoDealerPro.Modules.Leads.Core.Enums;
using AutoDealerPro.Modules.Leads.Core.Repositories;

namespace AutoDealerPro.Modules.Leads.Application.Services;

public class LeadsService(ILeadRepository repository) : ILeadsService
{
    private readonly ILeadRepository _repository = repository;

    public async Task<LeadDetailResponseV1> CreateLeadAsync(CreateLeadRequestV1 request)
    {
        Lead? existingLead = await _repository.GetByEmailAsync(request.Email);

        if (existingLead != null)
            throw new DuplicateLeadException(request.Email);

        Lead lead = Lead.Create(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Phone,
            request.VehicleId,
            request.Type,
            request.Message,
            request.TradeInMake,
            request.TradeInModel,
            request.TradeInYear,
            request.TradeInMileage
        );

        await _repository.AddAsync(lead);
        return lead.ToDetailResponse();
    }

    public async Task<LeadDetailResponseV1> GetLeadByIdAsync(Guid id)
    {
        Lead? lead = await _repository.GetByIdAsync(id) ?? 
            throw new LeadNotFoundException(id);
        return lead.ToDetailResponse();
    }

    public async Task<IEnumerable<LeadListResponseV1>> GetAllLeadsAsync(int page = 1, int pageSize = 10)
    {
        IEnumerable<Lead> leads = await _repository.GetAllAsync(page, pageSize);
        return leads.Select(x => x.ToListResponse());
    }

    public async Task<IEnumerable<LeadListResponseV1>> GetLeadsByStatusAsync(string status, int page = 1, int pageSize = 10)
    {
        if (!Enum.TryParse(status, true, out LeadStatus leadStatus))
            throw new ArgumentException($"Invalid lead status: {status}");

        IEnumerable<Lead> leads = await _repository.GetByStatusAsync(leadStatus, page, pageSize);
        return leads.Select(x => x.ToListResponse());
    }

    public async Task<IEnumerable<LeadListResponseV1>> GetLeadsByTypeAsync(string type, int page = 1, int pageSize = 10)
    {
        if (!Enum.TryParse(type, true, out LeadType leadType))
            throw new ArgumentException($"Invalid lead type: {type}");

        IEnumerable<Lead> leads = await _repository.GetByTypeAsync(leadType, page, pageSize);
        return leads.Select(x => x.ToListResponse());
    }

    public async Task<IEnumerable<LeadListResponseV1>> GetLeadsAssignedToStaffAsync(Guid staffId, int page = 1, int pageSize = 10)
    {
        IEnumerable<Lead> leads = await _repository.GetAssignedToStaffAsync(staffId, page, pageSize);
        return leads.Select(x => x.ToListResponse());
    }

    public async Task<IEnumerable<LeadListResponseV1>> GetLeadsByVehicleIdAsync(Guid vehicleId)
    {
        IEnumerable<Lead> leads = await _repository.GetByVehicleIdAsync(vehicleId);
        return leads.Select(x => x.ToListResponse());
    }

    public async Task<IEnumerable<LeadListResponseV1>> GetPendingFollowUpsAsync(int page = 1, int pageSize = 10)
    {
        IEnumerable<Lead> leads = await _repository.GetPendingFollowUpsAsync(page, pageSize);
        return leads.Select(x => x.ToListResponse());
    }

    public async Task AssignLeadToStaffAsync(Guid leadId, AssignLeadRequestV1 request)
    {
        Lead? lead = await _repository.GetByIdAsync(leadId) ?? 
            throw new LeadNotFoundException(leadId);

        lead.AssignToStaff(request.StaffId);
        await _repository.UpdateAsync(lead);
    }

    public async Task MarkLeadAsContactedAsync(Guid leadId, MarkAsContactedRequestV1 request)
    {
        Lead? lead = await _repository.GetByIdAsync(leadId) ?? 
            throw new LeadNotFoundException(leadId);

        lead.MarkAsContacted(request.Notes);
        await _repository.UpdateAsync(lead);
    }

    public async Task AddFollowUpAsync(Guid leadId, AddFollowUpRequestV1 request)
    {
        Lead? lead = await _repository.GetByIdAsync(leadId) ?? 
            throw new LeadNotFoundException(leadId);

        lead.AddFollowUp(request.Note, request.NextFollowUpDate);
        await _repository.UpdateAsync(lead);
    }

    public async Task CloseLeadAsync(Guid leadId, CloseLeadRequestv1 request)
    {
        Lead? lead = await _repository.GetByIdAsync(leadId) ?? 
            throw new LeadNotFoundException(leadId);

        lead.MarkAsClosed(request.Converted);
        await _repository.UpdateAsync(lead);
    }
}
