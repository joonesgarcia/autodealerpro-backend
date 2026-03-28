using AutoDealerPro.Modules.Leads.Application.DTOs;
using AutoDealerPro.Modules.Leads.Application.Exceptions;
using AutoDealerPro.Modules.Leads.Application.Interfaces;
using AutoDealerPro.Modules.Leads.Application.Requests;
using AutoDealerPro.Modules.Leads.Core.Entities;
using AutoDealerPro.Modules.Leads.Core.Enums;
using AutoDealerPro.Modules.Leads.Core.Repositories;

namespace AutoDealerPro.Modules.Leads.Application.Services;

public class LeadsService(ILeadRepository repository) : ILeadsService
{
    private readonly ILeadRepository _repository = repository;

    public async Task<LeadDetailDto> CreateLeadAsync(CreateLeadRequest request)
    {
        // Check for duplicate email
        var existingLead = await _repository.GetByEmailAsync(request.Email);
        if (existingLead != null)
            throw new DuplicateLeadException(request.Email);

        // Validate lead type
        if (!Enum.TryParse<LeadType>(request.Type, true, out var leadType))
            throw new ArgumentException($"Invalid lead type: {request.Type}");

        // Create lead
        var lead = Lead.Create(
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
        return MapToDetailDto(lead);
    }

    public async Task<LeadDetailDto> GetLeadByIdAsync(Guid id)
    {
        var lead = await _repository.GetByIdAsync(id);
        if (lead == null)
            throw new LeadNotFoundException(id);

        return MapToDetailDto(lead);
    }

    public async Task<IEnumerable<LeadListDto>> GetAllLeadsAsync(int page = 1, int pageSize = 10)
    {
        var leads = await _repository.GetAllAsync(page, pageSize);
        return leads.Select(MapToListDto);
    }

    public async Task<IEnumerable<LeadListDto>> GetLeadsByStatusAsync(string status, int page = 1, int pageSize = 10)
    {
        if (!Enum.TryParse<LeadStatus>(status, true, out var leadStatus))
            throw new ArgumentException($"Invalid lead status: {status}");

        var leads = await _repository.GetByStatusAsync(leadStatus, page, pageSize);
        return leads.Select(MapToListDto);
    }

    public async Task<IEnumerable<LeadListDto>> GetLeadsByTypeAsync(string type, int page = 1, int pageSize = 10)
    {
        if (!Enum.TryParse<LeadType>(type, true, out var leadType))
            throw new ArgumentException($"Invalid lead type: {type}");

        var leads = await _repository.GetByTypeAsync(leadType, page, pageSize);
        return leads.Select(MapToListDto);
    }

    public async Task<IEnumerable<LeadListDto>> GetLeadsAssignedToStaffAsync(Guid staffId, int page = 1, int pageSize = 10)
    {
        var leads = await _repository.GetAssignedToStaffAsync(staffId, page, pageSize);
        return leads.Select(MapToListDto);
    }

    public async Task<IEnumerable<LeadListDto>> GetLeadsByVehicleIdAsync(Guid vehicleId)
    {
        var leads = await _repository.GetByVehicleIdAsync(vehicleId);
        return leads.Select(MapToListDto);
    }

    public async Task<IEnumerable<LeadListDto>> GetPendingFollowUpsAsync(int page = 1, int pageSize = 10)
    {
        var leads = await _repository.GetPendingFollowUpsAsync(page, pageSize);
        return leads.Select(MapToListDto);
    }

    public async Task AssignLeadToStaffAsync(Guid leadId, AssignLeadRequest request)
    {
        var lead = await _repository.GetByIdAsync(leadId);
        if (lead == null)
            throw new LeadNotFoundException(leadId);

        lead.AssignToStaff(request.StaffId);
        await _repository.UpdateAsync(lead);
    }

    public async Task MarkLeadAsContactedAsync(Guid leadId, MarkAsContactedRequest request)
    {
        var lead = await _repository.GetByIdAsync(leadId);
        if (lead == null)
            throw new LeadNotFoundException(leadId);

        lead.MarkAsContacted(request.Notes);
        await _repository.UpdateAsync(lead);
    }

    public async Task AddFollowUpAsync(Guid leadId, AddFollowUpRequest request)
    {
        var lead = await _repository.GetByIdAsync(leadId);
        if (lead == null)
            throw new LeadNotFoundException(leadId);

        lead.AddFollowUp(request.Notes, request.NextFollowUpDate);
        await _repository.UpdateAsync(lead);
    }

    public async Task CloseLeadAsync(Guid leadId, CloseLeadRequest request)
    {
        var lead = await _repository.GetByIdAsync(leadId);
        if (lead == null)
            throw new LeadNotFoundException(leadId);

        lead.MarkAsClosed(request.Converted);
        await _repository.UpdateAsync(lead);
    }

    // Helper methods
    private static LeadDetailDto MapToDetailDto(Lead lead)
    {
        var followUpDtos = lead.FollowUps.Select(f => new FollowUpDto(
            f.Id,
            f.Notes,
            f.CreatedAt,
            f.NextFollowUpDate
        )).ToList();

        return new LeadDetailDto(
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
            followUpDtos,
            lead.CreatedAt,
            lead.UpdatedAt ?? lead.CreatedAt
        );
    }

    private static LeadListDto MapToListDto(Lead lead)
    {
        return new LeadListDto(
            lead.Id,
            lead.FirstName,
            lead.LastName,
            lead.Email,
            lead.Phone,
            lead.Type.ToString(),
            lead.Status.ToString(),
            lead.Message,
            lead.AssignedToStaffId,
            lead.CreatedAt
        );
    }
}
