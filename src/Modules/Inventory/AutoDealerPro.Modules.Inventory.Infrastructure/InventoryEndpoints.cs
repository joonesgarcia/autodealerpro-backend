using AutoDealerPro.Modules.Inventory.Application.Interfaces;
using AutoDealerPro.Modules.Inventory.Application.Requests;
using AutoDealerPro.Modules.Inventory.Application.Response;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;

namespace AutoDealerPro.Modules.Inventory.Infrastructure;

public static class InventoryEndpoints
{
    public static void MapInventoryEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/vehicles")
            .WithTags("Vehicles");


        group.MapGet("", async ([FromServices] IInventoryService service, int page, int pageSize) => {
            var vehicles = await service.GetAvailableVehiclesAsync(page, pageSize);
            return Results.Ok(vehicles);
        })
        .AllowAnonymous()
        .WithName("GetAvailableVehicles")
        .WithSummary("Browse available vehicles")
        .Produces<IEnumerable<VehicleListResponse>>();

        group.MapGet("{id:guid}", async (Guid id, [FromServices] IInventoryService service) => {
            var vehicle = await service.GetVehicleByIdAsync(id);
            if (vehicle == null) return Results.NotFound();
            return Results.Ok(vehicle);
        })
        .AllowAnonymous()
        .WithName("GetVehicleById")
        .WithSummary("Get vehicle details")
        .Produces<VehicleDetailResponse>()
        .Produces(404);

        group.MapGet("search", async ([FromServices] IInventoryService service, [AsParameters] VehicleSearchFilterRequest filter) => {
            var vehicles = await service.SearchVehiclesAsync(filter);
            return Results.Ok(vehicles);
        })
        .AllowAnonymous()
        .WithName("SearchVehicles")
        .WithSummary("Search and filter vehicles")
        .Produces<IEnumerable<VehicleListResponse>>();

        group.MapPost("", async ([FromServices] IInventoryService service, [FromServices] IValidator<CreateVehicleRequest> validator, CreateVehicleRequest request) => {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
                return Results.BadRequest(validation.Errors);
            var vehicle = await service.CreateVehicleAsync(request);
            return Results.Created($"/api/vehicles/{vehicle.Id}", vehicle);
        })
        .RequireAuthorization("StaffOnly")
        .WithName("CreateVehicle")
        .WithSummary("Add new vehicle (staff only)")
        .Produces<VehicleStaffResponse>(201)
        .Produces(400)
        .Produces(401);

        group.MapPut("{id:guid}/price", async (Guid id, UpdatePriceRequest request, [FromServices] IInventoryService service, [FromServices] IValidator<UpdatePriceRequest> validator) => {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
                return Results.BadRequest(validation.Errors);
            try {
                await service.UpdatePriceAsync(id, request);
                return Results.NoContent();
            } catch (ArgumentException) {
                return Results.NotFound();
            }
        })
        .RequireAuthorization("StaffOnly")
        .WithName("UpdateVehiclePrice")
        .WithSummary("Update asking price (staff only)")
        .Produces(204)
        .Produces(404)
        .Produces(401);

        group.MapPut("{id:guid}/mileage", async (Guid id, UpdateMileageRequest request, [FromServices] IInventoryService service, [FromServices] IValidator<UpdateMileageRequest> validator) => {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
                return Results.BadRequest(validation.Errors);
            try {
                await service.UpdateMileageAsync(id, request);
                return Results.NoContent();
            } catch (ArgumentException) {
                return Results.NotFound();
            }
        })
        .RequireAuthorization("StaffOnly")
        .WithName("UpdateVehicleMileage")
        .WithSummary("Update mileage (staff only)")
        .Produces(204)
        .Produces(404)
        .Produces(401);

        group.MapPost("{id:guid}/photos", async (Guid id, AddPhotoRequest request, [FromServices] IInventoryService service, [FromServices] IValidator<AddPhotoRequest> validator) => {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
                return Results.BadRequest(validation.Errors);
            try {
                await service.AddPhotoAsync(id, request);
                return Results.NoContent();
            } catch (ArgumentException) {
                return Results.NotFound();
            }
        })
        .RequireAuthorization("StaffOnly")
        .WithName("AddVehiclePhoto")
        .WithSummary("Add photo URL (staff only)")
        .Produces(204)
        .Produces(404)
        .Produces(401);

        group.MapPost("{id:guid}/mark-sold", async (Guid id, MarkAsSoldRequest request, [FromServices] IInventoryService service, [FromServices] IValidator<MarkAsSoldRequest> validator) => {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
                return Results.BadRequest(validation.Errors);
            try {
                await service.MarkAsSoldAsync(id, request);
                return Results.NoContent();
            } catch (ArgumentException) {
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

    // HANDLER METHODS
    private static async Task<IResult> GetAvailableVehicles(IInventoryService service, int page = 1, int pageSize = 12)
    {
        var vehicles = await service.GetAvailableVehiclesAsync(page, pageSize);
        return Results.Ok(vehicles);
    }

    private static async Task<IResult> GetVehicleById(IInventoryService service, Guid id)
    {
        var vehicle = await service.GetVehicleByIdAsync(id);
        if (vehicle == null) return Results.NotFound();
        return Results.Ok(vehicle);
    }

    private static async Task<IResult> SearchVehicles(
        IInventoryService service,
        [AsParameters] VehicleSearchFilterRequest filter)
    {
        var vehicles = await service.SearchVehiclesAsync(filter);
        return Results.Ok(vehicles);
    }

    private static async Task<IResult> CreateVehicle(
        IInventoryService service,
        IValidator<CreateVehicleRequest> validator,
        CreateVehicleRequest request)
    {
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
            return Results.BadRequest(validation.Errors);
        var vehicle = await service.CreateVehicleAsync(request);
        return Results.Created($"/api/vehicles/{vehicle.Id}", vehicle);
    }

    private static async Task<IResult> UpdatePrice(
        IInventoryService service,
        IValidator<UpdatePriceRequest> validator,
        Guid id,
        UpdatePriceRequest request)
    {
        var validation = await validator.ValidateAsync(request);
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
    }

    private static async Task<IResult> UpdateMileage(
        IInventoryService service,
        IValidator<UpdateMileageRequest> validator,
        Guid id,
        UpdateMileageRequest request)
    {
        var validation = await validator.ValidateAsync(request);
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
    }

    private static async Task<IResult> AddPhoto(
        IInventoryService service,
        IValidator<AddPhotoRequest> validator,
        Guid id,
        AddPhotoRequest request)
    {
        var validation = await validator.ValidateAsync(request);
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
    }

    private static async Task<IResult> MarkAsSold(
        IInventoryService service,
        IValidator<MarkAsSoldRequest> validator,
        Guid id,
        MarkAsSoldRequest request)
    {
        var validation = await validator.ValidateAsync(request);
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
    }

}