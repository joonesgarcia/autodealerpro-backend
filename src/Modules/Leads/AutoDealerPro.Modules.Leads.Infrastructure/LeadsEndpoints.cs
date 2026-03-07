using AutoDealerPro.Modules.Leads.Core.DTOs;
using AutoDealerPro.Modules.Leads.Core.Entities;
using AutoDealerPro.Modules.Leads.Core.Enums;
using AutoDealerPro.Modules.Leads.Core.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AutoDealerPro.Modules.Leads.Infrastructure
{
    public static class LeadsEndpoints
    {
        public static void MapLeadsEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("/api/leads")
                .WithTags("Leads");

            // PUBLIC ENDPOINTS

            group.MapPost("", CreateLead)
                .AllowAnonymous()
                .WithName("CreateLead")
                .WithSummary("Submit a new lead (customer)")
                .Produces<LeadDetailDto>(201)
                .Produces(400);

            group.MapGet("{id:guid}", GetLeadById)
                .AllowAnonymous()
                .WithName("GetLeadById")
                .WithSummary("Get lead details")
                .Produces<LeadDetailDto>()
                .Produces(404);

            // STAFF ENDPOINTS

            group.MapGet("", GetAllLeads)
                .RequireAuthorization("StaffOnly")
                .WithName("GetAllLeads")
                .WithSummary("Get all leads (staff only)")
                .Produces<IEnumerable<LeadListDto>>()
                .Produces(401);

            group.MapGet("status/{status}", GetLeadsByStatus)
                .RequireAuthorization("StaffOnly")
                .WithName("GetLeadsByStatus")
                .WithSummary("Get leads by status (staff only)")
                .Produces<IEnumerable<LeadListDto>>()
                .Produces(401);

            group.MapGet("type/{type}", GetLeadsByType)
                .RequireAuthorization("StaffOnly")
                .WithName("GetLeadsByType")
                .WithSummary("Get leads by type (staff only)")
                .Produces<IEnumerable<LeadListDto>>()
                .Produces(401);

            group.MapGet("staff/{staffId:guid}", GetLeadsAssignedToStaff)
                .RequireAuthorization("StaffOnly")
                .WithName("GetLeadsAssignedToStaff")
                .WithSummary("Get leads assigned to staff member (staff only)")
                .Produces<IEnumerable<LeadListDto>>()
                .Produces(401);

            group.MapGet("vehicle/{vehicleId:guid}", GetLeadsByVehicle)
                .RequireAuthorization("StaffOnly")
                .WithName("GetLeadsByVehicle")
                .WithSummary("Get all leads for a vehicle (staff only)")
                .Produces<IEnumerable<LeadListDto>>()
                .Produces(401);

            group.MapGet("pending-followups", GetPendingFollowUps)
                .RequireAuthorization("StaffOnly")
                .WithName("GetPendingFollowUps")
                .WithSummary("Get leads with pending follow-ups (staff only)")
                .Produces<IEnumerable<LeadListDto>>()
                .Produces(401);

            group.MapPut("{id:guid}/assign", AssignToStaff)
                .RequireAuthorization("StaffOnly")
                .WithName("AssignLeadToStaff")
                .WithSummary("Assign lead to staff member (staff only)")
                .Produces(204)
                .Produces(404)
                .Produces(401);

            group.MapPut("{id:guid}/contact", MarkAsContacted)
                .RequireAuthorization("StaffOnly")
                .WithName("MarkLeadAsContacted")
                .WithSummary("Mark lead as contacted with notes (staff only)")
                .Produces(204)
                .Produces(404)
                .Produces(401);

            group.MapPost("{id:guid}/followup", AddFollowUp)
                .RequireAuthorization("StaffOnly")
                .WithName("AddFollowUp")
                .WithSummary("Add follow-up to lead (staff only)")
                .Produces(204)
                .Produces(404)
                .Produces(401);

            group.MapPut("{id:guid}/close", CloseLead)
                .RequireAuthorization("StaffOnly")
                .WithName("CloseLead")
                .WithSummary("Close lead as converted or lost (staff only)")
                .Produces(204)
                .Produces(404)
                .Produces(401);
        }

        // PUBLIC ENDPOINT HANDLERS

        private static async Task<IResult> CreateLead(
            CreateLeadDto dto,
            ILeadRepository repository)
        {
            try
            {
                if (!Enum.TryParse<LeadType>(dto.Type, true, out var leadType))
                    return Results.BadRequest(new { error = "Invalid lead type" });

                var lead = Lead.Create(
                    dto.FirstName,
                    dto.LastName,
                    dto.Email,
                    dto.Phone,
                    dto.VehicleId,
                    leadType,
                    dto.Message,
                    dto.TradeInMake,
                    dto.TradeInModel,
                    dto.TradeInYear,
                    dto.TradeInMileage
                );

                await repository.AddAsync(lead);

                var responseDto = MapLeadToDetailDto(lead);
                return Results.Created($"/api/leads/{lead.Id}", responseDto);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        }

        private static async Task<IResult> GetLeadById(
            Guid id,
            ILeadRepository repository)
        {
            var lead = await repository.GetByIdAsync(id);
            if (lead == null)
                return Results.NotFound();

            return Results.Ok(MapLeadToDetailDto(lead));
        }

        // STAFF ENDPOINT HANDLERS

        private static async Task<IResult> GetAllLeads(
            ILeadRepository repository,
            int page = 1,
            int pageSize = 10)
        {
            var leads = await repository.GetAllAsync(page, pageSize);
            var dtos = leads.Select(l => MapLeadToListDto(l));
            return Results.Ok(dtos);
        }

        private static async Task<IResult> GetLeadsByStatus(
            string status,
            ILeadRepository repository,
            int page = 1,
            int pageSize = 10)
        {
            if (!Enum.TryParse<LeadStatus>(status, true, out var leadStatus))
                return Results.BadRequest(new { error = "Invalid lead status" });

            var leads = await repository.GetByStatusAsync(leadStatus, page, pageSize);
            var dtos = leads.Select(l => MapLeadToListDto(l));
            return Results.Ok(dtos);
        }

        private static async Task<IResult> GetLeadsByType(
            string type,
            ILeadRepository repository,
            int page = 1,
            int pageSize = 10)
        {
            if (!Enum.TryParse<LeadType>(type, true, out var leadType))
                return Results.BadRequest(new { error = "Invalid lead type" });

            var leads = await repository.GetByTypeAsync(leadType, page, pageSize);
            var dtos = leads.Select(l => MapLeadToListDto(l));
            return Results.Ok(dtos);
        }

        private static async Task<IResult> GetLeadsAssignedToStaff(
            Guid staffId,
            ILeadRepository repository,
            int page = 1,
            int pageSize = 10)
        {
            var leads = await repository.GetAssignedToStaffAsync(staffId, page, pageSize);
            var dtos = leads.Select(l => MapLeadToListDto(l));
            return Results.Ok(dtos);
        }

        private static async Task<IResult> GetLeadsByVehicle(
            Guid vehicleId,
            ILeadRepository repository)
        {
            var leads = await repository.GetByVehicleIdAsync(vehicleId);
            var dtos = leads.Select(l => MapLeadToListDto(l));
            return Results.Ok(dtos);
        }

        private static async Task<IResult> GetPendingFollowUps(
            ILeadRepository repository,
            int page = 1,
            int pageSize = 10)
        {
            var leads = await repository.GetPendingFollowUpsAsync(page, pageSize);
            var dtos = leads.Select(l => MapLeadToListDto(l));
            return Results.Ok(dtos);
        }

        private static async Task<IResult> AssignToStaff(
            Guid id,
            Guid staffId,
            ILeadRepository repository)
        {
            var lead = await repository.GetByIdAsync(id);
            if (lead == null)
                return Results.NotFound();

            lead.AssignToStaff(staffId);
            await repository.UpdateAsync(lead);

            return Results.NoContent();
        }

        private static async Task<IResult> MarkAsContacted(
            Guid id,
            string notes,
            ILeadRepository repository)
        {
            var lead = await repository.GetByIdAsync(id);
            if (lead == null)
                return Results.NotFound();

            lead.MarkAsContacted(notes);
            await repository.UpdateAsync(lead);

            return Results.NoContent();
        }

        private static async Task<IResult> AddFollowUp(
            Guid id,
            string notes,
            DateTime? nextFollowUpDate,
            ILeadRepository repository)
        {
            var lead = await repository.GetByIdAsync(id);
            if (lead == null)
                return Results.NotFound();

            lead.AddFollowUp(notes, nextFollowUpDate);
            await repository.UpdateAsync(lead);

            return Results.NoContent();
        }

        private static async Task<IResult> CloseLead(
            Guid id,
            bool converted,
            ILeadRepository repository)
        {
            var lead = await repository.GetByIdAsync(id);
            if (lead == null)
                return Results.NotFound();

            lead.MarkAsClosed(converted);
            await repository.UpdateAsync(lead);

            return Results.NoContent();
        }

        // HELPER METHODS

        private static LeadListDto MapLeadToListDto(Lead lead)
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

        private static LeadDetailDto MapLeadToDetailDto(Lead lead)
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
                lead.UpdatedAt
            );
        }
    }
}