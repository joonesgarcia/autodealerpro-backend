using AutoDealerPro.Modules.Inventory.Core.DTOs;
using AutoDealerPro.Modules.Inventory.Core.Entities;
using AutoDealerPro.Modules.Inventory.Core.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AutoDealerPro.Modules.Inventory.Infrastructure;

public static class InventoryEndpoints
{
    public static void MapInventoryEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/vehicles")
            .WithTags("Vehicles");

        // PUBLIC ENDPOINTS

        group.MapGet("", GetAvailableVehicles)
            .WithName("GetAvailableVehicles")
            .WithSummary("Browse available vehicles")
            .Produces<IEnumerable<VehicleListDto>>();

        group.MapGet("{id:guid}", GetVehicleById)
            .WithName("GetVehicleById")
            .WithSummary("Get vehicle details")
            .Produces<VehicleDetailDto>()
            .Produces(404);

        group.MapGet("search", SearchVehicles)
            .WithName("SearchVehicles")
            .WithSummary("Search and filter vehicles")
            .Produces<IEnumerable<VehicleListDto>>();

        // STAFF ENDPOINTS (will add [Authorize] later)

        group.MapPost("", CreateVehicle)
            .WithName("CreateVehicle")
            .WithSummary("Add new vehicle (staff only)")
            .Produces<VehicleStaffDto>(201)
            .Produces(400);

        group.MapPut("{id:guid}/price", UpdatePrice)
            .WithName("UpdateVehiclePrice")
            .WithSummary("Update asking price (staff only)")
            .Produces(204)
            .Produces(404);

        group.MapPut("{id:guid}/mileage", UpdateMileage)
            .WithName("UpdateVehicleMileage")
            .WithSummary("Update mileage (staff only)")
            .Produces(204)
            .Produces(404);

        group.MapPost("{id:guid}/photos", AddPhoto)
            .WithName("AddVehiclePhoto")
            .WithSummary("Add photo URL (staff only)")
            .Produces(204)
            .Produces(404);

        group.MapPost("{id:guid}/mark-sold", MarkAsSold)
            .WithName("MarkVehicleAsSold")
            .WithSummary("Mark vehicle as sold (staff only)")
            .Produces(204)
            .Produces(404);
    }

    // PUBLIC ENDPOINT HANDLERS

    private static async Task<IResult> GetAvailableVehicles(
        IVehicleRepository repository,
        int page = 1,
        int pageSize = 12)
    {
        var vehicles = await repository.GetAvailableAsync(page, pageSize);
        var dtos = vehicles.Select(v => new VehicleListDto(
            v.Id,
            v.Make,
            v.Model,
            v.Year,
            v.Trim,
            v.Mileage,
            v.ExteriorColor,
            v.Transmission,
            v.FuelType,
            v.BodyType,
            v.AskingPrice,
            v.PhotoUrls.FirstOrDefault() ?? "",
            v.ViewCount
        ));

        return Results.Ok(dtos);
    }

    private static async Task<IResult> GetVehicleById(
        Guid id,
        IVehicleRepository repository)
    {
        var vehicle = await repository.GetByIdAsync(id);
        if (vehicle == null)
            return Results.NotFound();

        // Increment view count
        vehicle.IncrementViewCount();
        await repository.UpdateAsync(vehicle);

        var dto = new VehicleDetailDto(
            vehicle.Id,
            vehicle.Make,
            vehicle.Model,
            vehicle.Year,
            vehicle.PlateNumber,
            vehicle.Trim,
            vehicle.Mileage,
            vehicle.ExteriorColor,
            vehicle.InteriorColor,
            vehicle.Transmission,
            vehicle.FuelType,
            vehicle.BodyType,
            vehicle.AskingPrice,
            vehicle.Status.ToString(),
            vehicle.PhotoUrls,
            vehicle.ViewCount,
            vehicle.CreatedAt
        );

        return Results.Ok(dto);
    }

    private static async Task<IResult> SearchVehicles(
        IVehicleRepository repository,
        string? make = null,
        string? model = null,
        int? minYear = null,
        int? maxYear = null,
        decimal? maxPrice = null,
        int? maxMileage = null,
        string? bodyType = null,
        string? fuelType = null)
    {
        var filter = new VehicleSearchFilterDto
        {
            Make = make,
            Model = model,
            MinYear = minYear,
            MaxYear = maxYear,
            MaxPrice = maxPrice,
            MaxMileage = maxMileage,
            BodyType = bodyType,
            FuelType = fuelType
        };

        var vehicles = await repository.SearchAsync(filter);
        var dtos = vehicles.Select(v => new VehicleListDto(
            v.Id, v.Make, v.Model, v.Year, v.Trim, v.Mileage,
            v.ExteriorColor, v.Transmission, v.FuelType, v.BodyType,
            v.AskingPrice, v.PhotoUrls.FirstOrDefault() ?? "", v.ViewCount
        ));

        return Results.Ok(dtos);
    }

    // STAFF ENDPOINT HANDLERS

    private static async Task<IResult> CreateVehicle(
        CreateVehicleDto dto,
        IVehicleRepository repository)
    {
        try
        {
            var vehicle = Vehicle.Create(
                dto.Make, dto.Model, dto.Year, dto.PlateNumber, dto.Trim,
                dto.Mileage, dto.ExteriorColor, dto.InteriorColor,
                dto.Transmission, dto.FuelType, dto.BodyType,
                dto.PurchasePrice, dto.AskingPrice, dto.Notes
            );

            await repository.AddAsync(vehicle);

            var responseDto = new VehicleStaffDto(
                vehicle.Id, vehicle.Make, vehicle.Model, vehicle.Year,
                vehicle.PlateNumber, vehicle.Trim, vehicle.Mileage,
                vehicle.ExteriorColor, vehicle.InteriorColor,
                vehicle.Transmission, vehicle.FuelType, vehicle.BodyType,
                vehicle.PurchasePrice, vehicle.AskingPrice, vehicle.SellingPrice,
                vehicle.Status.ToString(), vehicle.Notes, vehicle.PhotoUrls,
                vehicle.ViewCount, vehicle.CreatedAt, vehicle.SoldAt
            );

            return Results.Created($"/api/vehicles/{vehicle.Id}", responseDto);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    }

    private static async Task<IResult> UpdatePrice(
        Guid id,
        decimal newPrice,
        IVehicleRepository repository)
    {
        var vehicle = await repository.GetByIdAsync(id);
        if (vehicle == null)
            return Results.NotFound();

        vehicle.UpdatePrice(newPrice);
        await repository.UpdateAsync(vehicle);

        return Results.NoContent();
    }

    private static async Task<IResult> UpdateMileage(
        Guid id,
        int newMileage,
        IVehicleRepository repository)
    {
        var vehicle = await repository.GetByIdAsync(id);
        if (vehicle == null)
            return Results.NotFound();

        try
        {
            vehicle.UpdateMileage(newMileage);
            await repository.UpdateAsync(vehicle);
            return Results.NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    }

    private static async Task<IResult> AddPhoto(
        Guid id,
        string photoUrl,
        IVehicleRepository repository)
    {
        var vehicle = await repository.GetByIdAsync(id);
        if (vehicle == null)
            return Results.NotFound();

        try
        {
            vehicle.AddPhoto(photoUrl);
            await repository.UpdateAsync(vehicle);
            return Results.NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    }

    private static async Task<IResult> MarkAsSold(
        Guid id,
        decimal sellingPrice,
        IVehicleRepository repository)
    {
        var vehicle = await repository.GetByIdAsync(id);
        if (vehicle == null)
            return Results.NotFound();

        try
        {
            vehicle.MarkAsSold(sellingPrice);
            await repository.UpdateAsync(vehicle);
            return Results.NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    }
}