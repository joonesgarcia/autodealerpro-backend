using AutoDealerPro.Modules.Leads.Application.Exceptions;
using AutoDealerPro.Modules.Leads.Application.Interfaces;
using AutoDealerPro.Modules.Leads.Application.Requests;
using AutoDealerPro.Modules.Leads.Application.Response;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;

namespace AutoDealerPro.Modules.Leads.Infrastructure
{
    public static class LeadsEndpoints
    {
        public static void MapLeadsEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("/api/leads")
                .WithTags("Leads");



            group.MapPost("", async (CreateLeadRequest request, [FromServices] ILeadsService service, [FromServices] IValidator<CreateLeadRequest> validator) => {
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                    return Results.BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });
                var result = await service.CreateLeadAsync(request);
                return Results.Created($"/api/leads/{result.Id}", result);
            })
                .AllowAnonymous()
                .WithName("CreateLead")
                .WithSummary("Submit a new lead (customer)")
                .Produces<LeadDetailResponse>(201)
                .Produces(400);

            group.MapGet("{id:guid}", async (Guid id, [FromServices] ILeadsService service) => {
                try {
                    var result = await service.GetLeadByIdAsync(id);
                    return Results.Ok(result);
                } catch (LeadNotFoundException) {
                    return Results.NotFound();
                }
            })
                .AllowAnonymous()
                .WithName("GetLeadById")
                .WithSummary("Get lead details")
                .Produces<LeadDetailResponse>()
                .Produces(404);

            group.MapGet("", async ([FromServices] ILeadsService service, int page, int pageSize) => {
                var leads = await service.GetAllLeadsAsync(page, pageSize);
                return Results.Ok(leads);
            })
                .RequireAuthorization("StaffOnly")
                .WithName("GetAllLeads")
                .WithSummary("Get all leads (staff only)")
                .Produces<IEnumerable<LeadListResponse>>()
                .Produces(401);

            group.MapGet("status/{status}", async (string status, [FromServices] ILeadsService service, int page, int pageSize) => {
                try {
                    var leads = await service.GetLeadsByStatusAsync(status, page, pageSize);
                    return Results.Ok(leads);
                } catch (ArgumentException ex) {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
                .RequireAuthorization("StaffOnly")
                .WithName("GetLeadsByStatus")
                .WithSummary("Get leads by status (staff only)")
                .Produces<IEnumerable<LeadListResponse>>()
                .Produces(401);

            group.MapGet("type/{type}", async (string type, [FromServices] ILeadsService service, int page, int pageSize) => {
                try {
                    var leads = await service.GetLeadsByTypeAsync(type, page, pageSize);
                    return Results.Ok(leads);
                } catch (ArgumentException ex) {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
                .RequireAuthorization("StaffOnly")
                .WithName("GetLeadsByType")
                .WithSummary("Get leads by type (staff only)")
                .Produces<IEnumerable<LeadListResponse>>()
                .Produces(401);

            group.MapGet("staff/{staffId:guid}", async (Guid staffId, [FromServices] ILeadsService service, int page, int pageSize) => {
                var leads = await service.GetLeadsAssignedToStaffAsync(staffId, page, pageSize);
                return Results.Ok(leads);
            })
                .RequireAuthorization("StaffOnly")
                .WithName("GetLeadsAssignedToStaff")
                .WithSummary("Get leads assigned to staff member (staff only)")
                .Produces<IEnumerable<LeadListResponse>>()
                .Produces(401);

            group.MapGet("vehicle/{vehicleId:guid}", async (Guid vehicleId, [FromServices] ILeadsService service) => {
                var leads = await service.GetLeadsByVehicleIdAsync(vehicleId);
                return Results.Ok(leads);
            })
                .RequireAuthorization("StaffOnly")
                .WithName("GetLeadsByVehicle")
                .WithSummary("Get all leads for a vehicle (staff only)")
                .Produces<IEnumerable<LeadListResponse>>()
                .Produces(401);

            group.MapGet("pending-followups", async ([FromServices] ILeadsService service, int page, int pageSize) => {
                var leads = await service.GetPendingFollowUpsAsync(page, pageSize);
                return Results.Ok(leads);
            })
                .RequireAuthorization("StaffOnly")
                .WithName("GetPendingFollowUps")
                .WithSummary("Get leads with pending follow-ups (staff only)")
                .Produces<IEnumerable<LeadListResponse>>()
                .Produces(401);


            group.MapPut("{id:guid}/assign", async (Guid id, AssignLeadRequest request, [FromServices] ILeadsService service, [FromServices] IValidator<AssignLeadRequest> validator) => {
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                    return Results.BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });
                await service.AssignLeadToStaffAsync(id, request);
                return Results.NoContent();
            })
                .RequireAuthorization("StaffOnly")
                .WithName("AssignLeadToStaff")
                .WithSummary("Assign lead to staff member (staff only)")
                .Produces(204)
                .Produces(404)
                .Produces(401);

            group.MapPut("{id:guid}/contact", async (Guid id, MarkAsContactedRequest request, [FromServices] ILeadsService service, [FromServices] IValidator<MarkAsContactedRequest> validator) => {
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                    return Results.BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });
                await service.MarkLeadAsContactedAsync(id, request);
                return Results.NoContent();
            })
                .RequireAuthorization("StaffOnly")
                .WithName("MarkLeadAsContacted")
                .WithSummary("Mark lead as contacted with notes (staff only)")
                .Produces(204)
                .Produces(404)
                .Produces(401);

            group.MapPost("{id:guid}/followup", async (Guid id, AddFollowUpRequest request, [FromServices] ILeadsService service, [FromServices] IValidator<AddFollowUpRequest> validator) => {
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                    return Results.BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });
                await service.AddFollowUpAsync(id, request);
                return Results.NoContent();
            })
                .RequireAuthorization("StaffOnly")
                .WithName("AddFollowUp")
                .WithSummary("Add follow-up to lead (staff only)")
                .Produces(204)
                .Produces(404)
                .Produces(401);

            group.MapPut("{id:guid}/close", async (Guid id, CloseLeadRequest request, [FromServices] ILeadsService service, [FromServices] IValidator<CloseLeadRequest> validator) => {
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                    return Results.BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });
                await service.CloseLeadAsync(id, request);
                return Results.NoContent();
            })
                .RequireAuthorization("StaffOnly")
                .WithName("CloseLead")
                .WithSummary("Close lead as converted or lost (staff only)")
                .Produces(204)
                .Produces(404)
                .Produces(401);
        }

        // PUBLIC ENDPOINT HANDLERS

        private static async Task<IResult> CreateLead(
            CreateLeadRequest request,
            ILeadsService service,
            IValidator<CreateLeadRequest> validator)
        {
            try
            {
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                    return Results.BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });

                var result = await service.CreateLeadAsync(request);
                return Results.Created($"/api/leads/{result.Id}", result);
            }
            catch (DuplicateLeadException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        }

        private static async Task<IResult> GetLeadById(
            Guid id,
            [FromServices] ILeadsService service)
        {
            try
            {
                var result = await service.GetLeadByIdAsync(id);
                return Results.Ok(result);
            }
            catch (LeadNotFoundException)
            {
                return Results.NotFound();
            }
        }

        // STAFF ENDPOINT HANDLERS

        private static async Task<IResult> GetAllLeads(
            [FromServices] ILeadsService service,
            int page = 1,
            int pageSize = 10)
        {
            var leads = await service.GetAllLeadsAsync(page, pageSize);
            return Results.Ok(leads);
        }

        private static async Task<IResult> GetLeadsByStatus(
            string status,
            ILeadsService service,
            int page = 1,
            int pageSize = 10)
        {
            try
            {
                var leads = await service.GetLeadsByStatusAsync(status, page, pageSize);
                return Results.Ok(leads);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        }

        private static async Task<IResult> GetLeadsByType(
            string type,
            [FromServices] ILeadsService service,
            int page = 1,
            int pageSize = 10)
        {
            try
            {
                var leads = await service.GetLeadsByTypeAsync(type, page, pageSize);
                return Results.Ok(leads);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        }

        private static async Task<IResult> GetLeadsAssignedToStaff(
            Guid staffId,
            [FromServices] ILeadsService service,
            int page = 1,
            int pageSize = 10)
        {
            var leads = await service.GetLeadsAssignedToStaffAsync(staffId, page, pageSize);
            return Results.Ok(leads);
        }

        private static async Task<IResult> GetLeadsByVehicle(
            Guid vehicleId,
            [FromServices] ILeadsService service)
        {
            var leads = await service.GetLeadsByVehicleIdAsync(vehicleId);
            return Results.Ok(leads);
        }

        private static async Task<IResult> GetPendingFollowUps(
            [FromServices] ILeadsService service,
            int page = 1,
            int pageSize = 10)
        {
            var leads = await service.GetPendingFollowUpsAsync(page, pageSize);
            return Results.Ok(leads);
        }

        private static async Task<IResult> AssignToStaff(
            Guid id,
            AssignLeadRequest request,
            [FromServices] ILeadsService service,
            IValidator<AssignLeadRequest> validator)
        {
            try
            {
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                    return Results.BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });

                await service.AssignLeadToStaffAsync(id, request);
                return Results.NoContent();
            }
            catch (LeadNotFoundException)
            {
                return Results.NotFound();
            }
        }

        private static async Task<IResult> MarkAsContacted(
            Guid id,
            MarkAsContactedRequest request,
            [FromServices] ILeadsService service,
            IValidator<MarkAsContactedRequest> validator)
        {
            try
            {
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                    return Results.BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });

                await service.MarkLeadAsContactedAsync(id, request);
                return Results.NoContent();
            }
            catch (LeadNotFoundException)
            {
                return Results.NotFound();
            }
        }

        private static async Task<IResult> AddFollowUp(
            Guid id,
            AddFollowUpRequest request,
            ILeadsService service,
            IValidator<AddFollowUpRequest> validator)
        {
            try
            {
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                    return Results.BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });

                await service.AddFollowUpAsync(id, request);
                return Results.NoContent();
            }
            catch (LeadNotFoundException)
            {
                return Results.NotFound();
            }
        }

        private static async Task<IResult> CloseLead(
            Guid id,
            CloseLeadRequest request,
            ILeadsService service)
        {
            try
            {
                await service.CloseLeadAsync(id, request);
                return Results.NoContent();
            }
            catch (LeadNotFoundException)
            {
                return Results.NotFound();
            }
        }
    }
}