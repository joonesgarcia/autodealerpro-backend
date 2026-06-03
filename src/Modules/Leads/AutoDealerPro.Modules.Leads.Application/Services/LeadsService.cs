using AutoDealerPro.Modules.Leads.Application.Exceptions;
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

        if (!Enum.TryParse<LeadType>(request.Type, true, out LeadType leadType))
            throw new ArgumentException($"Invalid lead type: {request.Type}");

        Lead lead = Lead.Create(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Phone,
            request.VehicleId,
            leadType,
            request.Message,
            request.TradeInMake,
            request.TradeInModel,
            request.TradeInYear,
            request.TradeInMileage
        );

        await _repository.AddAsync(lead);
        return MapToDetailResponse(lead);
    }

    public async Task<LeadDetailResponseV1> GetLeadByIdAsync(Guid id)
    {
        Lead? lead = await _repository.GetByIdAsync(id);
        if (lead == null)
            throw new LeadNotFoundException(id);

        return MapToDetailResponse(lead);
    }

    public async Task<IEnumerable<LeadListResponseV1>> GetAllLeadsAsync(int page = 1, int pageSize = 10)
    {
        IEnumerable<Lead> leads = await _repository.GetAllAsync(page, pageSize);
        return leads.Select(MapToListResponse);
    }

    public async Task<IEnumerable<LeadListResponseV1>> GetLeadsByStatusAsync(string status, int page = 1, int pageSize = 10)
    {
        if (!Enum.TryParse<LeadStatus>(status, true, out LeadStatus leadStatus))
            throw new ArgumentException($"Invalid lead status: {status}");

        IEnumerable<Lead> leads = await _repository.GetByStatusAsync(leadStatus, page, pageSize);
        return leads.Select(MapToListResponse);
    }

    public async Task<IEnumerable<LeadListResponseV1>> GetLeadsByTypeAsync(string type, int page = 1, int pageSize = 10)
    {
        if (!Enum.TryParse<LeadType>(type, true, out LeadType leadType))
            throw new ArgumentException($"Invalid lead type: {type}");

        IEnumerable<Lead> leads = await _repository.GetByTypeAsync(leadType, page, pageSize);
        return leads.Select(MapToListResponse);
    }

    public async Task<IEnumerable<LeadListResponseV1>> GetLeadsAssignedToStaffAsync(Guid staffId, int page = 1, int pageSize = 10)
    {
        IEnumerable<Lead> leads = await _repository.GetAssignedToStaffAsync(staffId, page, pageSize);
        return leads.Select(MapToListResponse);
    }

    public async Task<IEnumerable<LeadListResponseV1>> GetLeadsByVehicleIdAsync(Guid vehicleId)
    {
        IEnumerable<Lead> leads = await _repository.GetByVehicleIdAsync(vehicleId);
        return leads.Select(MapToListResponse);
    }

    public async Task<IEnumerable<LeadListResponseV1>> GetPendingFollowUpsAsync(int page = 1, int pageSize = 10)
    {
        IEnumerable<Lead> leads = await _repository.GetPendingFollowUpsAsync(page, pageSize);
        return leads.Select(MapToListResponse);
    }

    public async Task AssignLeadToStaffAsync(Guid leadId, AssignLeadRequestV1 request)
    {
        Lead? lead = await _repository.GetByIdAsync(leadId);
        if (lead == null)
            throw new LeadNotFoundException(leadId);

        lead.AssignToStaff(request.StaffId);
        await _repository.UpdateAsync(lead);
    }

    public async Task MarkLeadAsContactedAsync(Guid leadId, MarkAsContactedRequestV1 request)
    {
        Lead? lead = await _repository.GetByIdAsync(leadId);
        if (lead == null)
            throw new LeadNotFoundException(leadId);

        lead.MarkAsContacted(request.Notes);
        await _repository.UpdateAsync(lead);
    }

    public async Task AddFollowUpAsync(Guid leadId, AddFollowUpRequestV1 request)
    {
        Lead? lead = await _repository.GetByIdAsync(leadId);
        if (lead == null)
            throw new LeadNotFoundException(leadId);

        lead.AddFollowUp(request.Notes, request.NextFollowUpDate);
        await _repository.UpdateAsync(lead);
    }

    public async Task CloseLeadAsync(Guid leadId, CloseLeadRequestv1 request)
    {
        Lead? lead = await _repository.GetByIdAsync(leadId);
        if (lead == null)
            throw new LeadNotFoundException(leadId);

        lead.MarkAsClosed(request.Converted);
        await _repository.UpdateAsync(lead);
    }

    // Helper methods
    private static LeadDetailResponseV1 MapToDetailResponse(Lead lead)
    {
        List<FollowUpResponseV1> followUpResponses = lead.FollowUps.Select(f => new FollowUpResponseV1(
            f.Id,
            f.Notes,
            f.CreatedAt,
            f.NextFollowUpDate
        )).ToList();

        return new LeadDetailResponseV1(
            lead.Id,
            lead.FirstName,
            lead.LastName,
            lead.Email,
            lead.Phone,
            lead.VehicleId,
            lead.Type.ToString(),
            lead.Status.ToString(),
            lead.Message,
            lead.TradeInMake,
            lead.TradeInModel,
            lead.TradeInYear,
            lead.TradeInMileage,
            lead.AssignedToStaffId,
            lead.ContactedAt,
            lead.StaffNotes,
            followUpResponses,
            lead.CreatedAt,
            lead.UpdatedAt ?? lead.CreatedAt
        );
    }

    private static LeadListResponseV1 MapToListResponse(Lead lead)
    {
        return new LeadListResponseV1(
            lead.Id,
            lead.FirstName,
            lead.LastName,
            lead.Email,
            lead.Phone,
            lead.Type.ToString(),
            lead.Status.ToString(),
            lead.Message,
            lead.Priority.ToString(),
            lead.AssignedToStaffId,
            lead.CreatedAt
        );
    }
}
