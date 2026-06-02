using AutoDealerPro.Modules.Inventory.Application.Interfaces;
using AutoDealerPro.Modules.Inventory.Application.Requests.Filter;
using AutoDealerPro.Modules.Inventory.Application.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AutoDealerPro.Modules.Inventory.Infrastructure.Endpoints;

public static class VehicleQueryEndpoints
{
    public static void MapVehicleQueryRoutes(this IEndpointRouteBuilder endpoints)
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
    }
}