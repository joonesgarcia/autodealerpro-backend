using AutoDealerPro.Modules.Inventory.Application.Interfaces;
using AutoDealerPro.Modules.Inventory.Application.Requests.AddPhoto;
using AutoDealerPro.Modules.Inventory.Application.Requests.AddVehicle;
using AutoDealerPro.Modules.Inventory.Application.Requests.Filter;
using AutoDealerPro.Modules.Inventory.Application.Requests.MarkAsSold;
using AutoDealerPro.Modules.Inventory.Application.Requests.UpdateMileage;
using AutoDealerPro.Modules.Inventory.Application.Requests.UpdatePrice;
using AutoDealerPro.Modules.Inventory.Application.Responses;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AutoDealerPro.Modules.Inventory.Infrastructure;

public static class InventoryEndpoints
{
    public static void MapInventoryEndpoints(this IEndpointRouteBuilder endpoints)
    {
        RouteGroupBuilder group = endpoints.MapGroup("/api/vehicles")
            .WithTags("Vehicles");


        group.MapGet("", async ([FromServices] IInventoryService service, int page, int pageSize) =>
        {
            IEnumerable<VehicleBasicViewResponse> vehicles = await service.GetAvailableVehiclesAsync(page, pageSize);
            return Results.Ok(vehicles);
        })
        .AllowAnonymous()
        .WithName("GetAvailableVehicles")
        .WithSummary("Browse available vehicles")
        .Produces<IEnumerable<VehicleBasicViewResponse>>();

        group.MapGet("{id:guid}", async (Guid id, [FromServices] IInventoryService service) =>
        {
            VehicleDetailedViewResponse? vehicle = await service.GetVehicleByIdAsync(id);
            if (vehicle == null) return Results.NotFound();
            return Results.Ok(vehicle);
        })
        .AllowAnonymous()
        .WithName("GetVehicleById")
        .WithSummary("Get vehicle details")
        .Produces<VehicleDetailedViewResponse>()
        .Produces(404);

        group.MapGet("search", async ([FromServices] IInventoryService service, [AsParameters] VehicleFilterRequest filter) =>
        {
            IEnumerable<VehicleBasicViewResponse> vehicles = await service.SearchVehiclesAsync(filter);
            return Results.Ok(vehicles);
        })
        .AllowAnonymous()
        .WithName("SearchVehicles")
        .WithSummary("Search and filter vehicles")
        .Produces<IEnumerable<VehicleBasicViewResponse>>();

        group.MapPost("", async ([FromServices] IInventoryService service, [FromServices] IValidator<AddVehicleRequest> validator, AddVehicleRequest request) =>
        {
            FluentValidation.Results.ValidationResult validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
                return Results.BadRequest(validation.Errors);
            VehicleStaffViewResponse vehicle = await service.CreateVehicleAsync(request);
            return Results.Created($"/api/vehicles/{vehicle.Id}", vehicle);
        })
        .RequireAuthorization("StaffOnly")
        .WithName("CreateVehicle")
        .WithSummary("Add new vehicle (staff only)")
        .Produces<VehicleStaffViewResponse>(201)
        .Produces(400)
        .Produces(401);

        group.MapPut("{id:guid}/price", async (Guid id, UpdatePriceRequest request, [FromServices] IInventoryService service, [FromServices] IValidator<UpdatePriceRequest> validator) =>
        {
            FluentValidation.Results.ValidationResult validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
                return Results.BadRequest(validation.Errors);
            try
            {
                await service.UpdatePriceAsync(id, request);
                return Results.NoContent();
            }
            catch (ArgumentException)
            {
                return Results.NotFound();
            }
        })
        .RequireAuthorization("StaffOnly")
        .WithName("UpdateVehiclePrice")
        .WithSummary("Update asking price (staff only)")
        .Produces(204)
        .Produces(404)
        .Produces(401);

        group.MapPut("{id:guid}/mileage", async (Guid id, UpdateMileageRequest request, [FromServices] IInventoryService service, [FromServices] IValidator<UpdateMileageRequest> validator) =>
        {
            FluentValidation.Results.ValidationResult validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
                return Results.BadRequest(validation.Errors);
            try
            {
                await service.UpdateMileageAsync(id, request);
                return Results.NoContent();
            }
            catch (ArgumentException)
            {
                return Results.NotFound();
            }
        })
        .RequireAuthorization("StaffOnly")
        .WithName("UpdateVehicleMileage")
        .WithSummary("Update mileage (staff only)")
        .Produces(204)
        .Produces(404)
        .Produces(401);

        group.MapPost("{id:guid}/photos", async (Guid id, AddPhotoRequest request, [FromServices] IInventoryService service, [FromServices] IValidator<AddPhotoRequest> validator) =>
        {
            FluentValidation.Results.ValidationResult validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
                return Results.BadRequest(validation.Errors);
            try
            {
                await service.AddPhotoAsync(id, request);
                return Results.NoContent();
            }
            catch (ArgumentException)
            {
                return Results.NotFound();
            }
        })
        .RequireAuthorization("StaffOnly")
        .WithName("AddVehiclePhoto")
        .WithSummary("Add photo URL (staff only)")
        .Produces(204)
        .Produces(404)
        .Produces(401);

        group.MapPost("{id:guid}/mark-sold", async (Guid id, MarkAsSoldRequest request, [FromServices] IInventoryService service, [FromServices] IValidator<MarkAsSoldRequest> validator) =>
        {
            FluentValidation.Results.ValidationResult validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
                return Results.BadRequest(validation.Errors);
            try
            {
                await service.MarkAsSoldAsync(id, request);
                return Results.NoContent();
            }
            catch (ArgumentException)
            {
                return Results.NotFound();
            }
        })
        .RequireAuthorization("StaffOnly")
        .WithName("MarkVehicleAsSold")
        .WithSummary("Mark vehicle as sold (staff only)")
        .Produces(204)
        .Produces(404)
        .Produces(401);
    }
}