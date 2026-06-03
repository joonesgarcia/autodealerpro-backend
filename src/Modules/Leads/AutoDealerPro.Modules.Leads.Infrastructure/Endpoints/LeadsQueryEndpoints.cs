using AutoDealerPro.Modules.Leads.Application.Exceptions;
using AutoDealerPro.Modules.Leads.Application.Interfaces;
using AutoDealerPro.Modules.Leads.Application.Response.V1;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AutoDealerPro.Modules.Leads.Infrastructure.Endpoints
{
    public static class LeadsQueryEndpoints
    {
        public static void MapLeadsQueryRoutes(this IEndpointRouteBuilder endpoints)
        {
            RouteGroupBuilder group = endpoints.MapGroup("/api/v1/leads")
                .WithTags("Leads");

            group.MapGet("{id:guid}", async (Guid id, [FromServices] ILeadsService service) =>
            {
                try
                {
                    LeadDetailResponseV1 result = await service.GetLeadByIdAsync(id);
                    return Results.Ok(result);
                }
                catch (LeadNotFoundException)
                {
                    return Results.NotFound();
                }
            })
                .RequireAuthorization("StaffOnly")
                .WithName("GetLeadById")
                .WithSummary("Get lead details")
                .Produces<LeadDetailResponseV1>()
                .Produces(404);

            group.MapGet("", async ([FromServices] ILeadsService service, int page, int pageSize) =>
            {
                IEnumerable<LeadListResponseV1> leads = await service.GetAllLeadsAsync(page, pageSize);
                return Results.Ok(leads);
            })
                .RequireAuthorization("StaffOnly")
                .WithName("GetAllLeads")
                .WithSummary("Get all leads (staff only)")
                .Produces<IEnumerable<LeadListResponseV1>>()
                .Produces(401);

            group.MapGet("status/{status}", async (string status, [FromServices] ILeadsService service, int page, int pageSize) =>
            {
                try
                {
                    IEnumerable<LeadListResponseV1> leads = await service.GetLeadsByStatusAsync(status, page, pageSize);
                    return Results.Ok(leads);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
                .RequireAuthorization("StaffOnly")
                .WithName("GetLeadsByStatus")
                .WithSummary("Get leads by status (staff only)")
                .Produces<IEnumerable<LeadListResponseV1>>()
                .Produces(401);

            group.MapGet("type/{type}", async (string type, [FromServices] ILeadsService service, int page, int pageSize) =>
            {
                try
                {
                    IEnumerable<LeadListResponseV1> leads = await service.GetLeadsByTypeAsync(type, page, pageSize);
                    return Results.Ok(leads);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
                .RequireAuthorization("StaffOnly")
                .WithName("GetLeadsByType")
                .WithSummary("Get leads by type (staff only)")
                .Produces<IEnumerable<LeadListResponseV1>>()
                .Produces(401);

            group.MapGet("staff/{staffId:guid}", async (Guid staffId, [FromServices] ILeadsService service, int page, int pageSize) =>
            {
                IEnumerable<LeadListResponseV1> leads = await service.GetLeadsAssignedToStaffAsync(staffId, page, pageSize);
                return Results.Ok(leads);
            })
                .RequireAuthorization("StaffOnly")
                .WithName("GetLeadsAssignedToStaff")
                .WithSummary("Get leads assigned to staff member (staff only)")
                .Produces<IEnumerable<LeadListResponseV1>>()
                .Produces(401);

            group.MapGet("vehicle/{vehicleId:guid}", async (Guid vehicleId, [FromServices] ILeadsService service) =>
            {
                IEnumerable<LeadListResponseV1> leads = await service.GetLeadsByVehicleIdAsync(vehicleId);
                return Results.Ok(leads);
            })
                .RequireAuthorization("StaffOnly")
                .WithName("GetLeadsByVehicle")
                .WithSummary("Get all leads for a vehicle (staff only)")
                .Produces<IEnumerable<LeadListResponseV1>>()
                .Produces(401);

            group.MapGet("pending-followups", async ([FromServices] ILeadsService service, int page, int pageSize) =>
            {
                IEnumerable<LeadListResponseV1> leads = await service.GetPendingFollowUpsAsync(page, pageSize);
                return Results.Ok(leads);
            })
                .RequireAuthorization("StaffOnly")
                .WithName("GetPendingFollowUps")
                .WithSummary("Get leads with pending follow-ups (staff only)")
                .Produces<IEnumerable<LeadListResponseV1>>()
                .Produces(401);
        }
    }
}