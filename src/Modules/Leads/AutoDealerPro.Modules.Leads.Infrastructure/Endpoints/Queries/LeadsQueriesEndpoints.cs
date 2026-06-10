using AutoDealerPro.Modules.Leads.Application.Exceptions;
using AutoDealerPro.Modules.Leads.Application.V1.Queries.GetAllLeads;
using AutoDealerPro.Modules.Leads.Application.V1.Queries.GetLeadById;
using AutoDealerPro.Modules.Leads.Application.V1.Queries.GetLeadsAssignedToStaff;
using AutoDealerPro.Modules.Leads.Application.V1.Queries.GetLeadsByStatus;
using AutoDealerPro.Modules.Leads.Application.V1.Queries.GetLeadsByType;
using AutoDealerPro.Modules.Leads.Application.V1.Queries.GetLeadsByVehicleIdQuery;
using AutoDealerPro.Modules.Leads.Application.V1.Queries.GetPendingFollowUps;
using AutoDealerPro.Modules.Leads.Application.V1.Response;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AutoDealerPro.Modules.Leads.Infrastructure.Endpoints.Queries
{
    public static class LeadsQueriesEndpoints
    {
        public static void MapLeadsQueryRoutes(this IEndpointRouteBuilder endpoints)
        {
            RouteGroupBuilder group = endpoints.MapGroup("/api/v1/leads")
                .WithTags("Leads");

            group.MapGet("{id:guid}", async (Guid id, [FromServices] IMediator mediator, CancellationToken ct) =>
            {
                try
                {
                    var result = await mediator.Send(new GetLeadByIdV1Query(id), ct);
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
                .Produces<LeadDetailResponseV1>(200)
                .Produces(404);

            group.MapGet("", async ([FromServices] IMediator mediator, CancellationToken ct, int page = 1, int pageSize = 10) =>
            {
                var leads = await mediator.Send(new GetAllLeadsV1Query(page, pageSize), ct);
                return Results.Ok(leads);
            })
                .RequireAuthorization("StaffOnly")
                .WithName("GetAllLeads")
                .WithSummary("Get all leads (staff only)")
                .Produces<IEnumerable<LeadListResponseV1>>(200)
                .Produces(401);

            group.MapGet("status/{status}", async (string status, [FromServices] IMediator mediator, CancellationToken ct, int page = 1, int pageSize = 10) =>
            {
                try
                {
                    var leads = await mediator.Send(new GetLeadsByStatusV1Query(status, page, pageSize), ct);
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
                .Produces<IEnumerable<LeadListResponseV1>>(200)
                .Produces(401);

            group.MapGet("type/{type}", async (string type, [FromServices] IMediator mediator, CancellationToken ct, int page = 1, int pageSize = 10) =>
            {
                try
                {
                    var leads = await mediator.Send(new GetLeadsByTypeV1Query(type, page, pageSize), ct);
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
                .Produces<IEnumerable<LeadListResponseV1>>(200)
                .Produces(401);

            group.MapGet("staff/{staffId:guid}", async (Guid staffId, [FromServices] IMediator mediator, CancellationToken ct, int page = 1, int pageSize = 10) =>
            {
                var leads = await mediator.Send(new GetLeadsAssignedToStaffV1Query(staffId, page, pageSize), ct);
                return Results.Ok(leads);
            })
                .RequireAuthorization("StaffOnly")
                .WithName("GetLeadsAssignedToStaff")
                .WithSummary("Get leads assigned to staff member (staff only)")
                .Produces<IEnumerable<LeadListResponseV1>>(200)
                .Produces(401);

            group.MapGet("vehicle/{vehicleId:guid}", async (Guid vehicleId, [FromServices] IMediator mediator, CancellationToken ct) =>
            {
                var leads = await mediator.Send(new GetLeadsByVehicleIdV1Query(vehicleId), ct);
                return Results.Ok(leads);
            })
                .RequireAuthorization("StaffOnly")
                .WithName("GetLeadsByVehicle")
                .WithSummary("Get all leads for a vehicle (staff only)")
                .Produces<IEnumerable<LeadListResponseV1>>(200)
                .Produces(401);

            group.MapGet("pending-followups", async ([FromServices] IMediator mediator, CancellationToken ct, int page = 1, int pageSize = 10) =>
            {
                var leads = await mediator.Send(new GetPendingFollowUpsV1Query(page, pageSize), ct);
                return Results.Ok(leads);
            })
                .RequireAuthorization("StaffOnly")
                .WithName("GetPendingFollowUps")
                .WithSummary("Get leads with pending follow-ups (staff only)")
                .Produces<IEnumerable<LeadListResponseV1>>(200)
                .Produces(401);
        }
    }
}