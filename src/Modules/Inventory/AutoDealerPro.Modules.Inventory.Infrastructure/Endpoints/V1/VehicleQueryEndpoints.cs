using AutoDealerPro.Modules.Inventory.Application.Interfaces;
using AutoDealerPro.Modules.Inventory.Application.Requests.V1;
using AutoDealerPro.Modules.Inventory.Application.Responses.V1;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AutoDealerPro.Modules.Inventory.Infrastructure.Endpoints.V1;

public static class VehicleQueryEndpoints
{
    public static void MapVehicleQueryRoutes(this IEndpointRouteBuilder endpoints)
    {
        RouteGroupBuilder group = endpoints.MapGroup("/api/v1/vehicles")
            .WithTags("Vehicles");

        group.MapGet("", async ([FromServices] IInventoryService service, int page, int pageSize) =>
        {
            IEnumerable<VehicleBasicViewResponseV1> vehicles = await service.GetAvailableVehiclesAsync(page, pageSize);
            return Results.Ok(vehicles);
        })
        .AllowAnonymous()
        .WithName("GetAvailableVehicles")
        .WithSummary("Browse available vehicles")
        .Produces<IEnumerable<VehicleBasicViewResponseV1>>();

        group.MapGet("{id:guid}", async (Guid id, [FromServices] IInventoryService service) =>
        {
            VehicleDetailedViewResponseV1? vehicle = await service.GetVehicleByIdAsync(id);
            if (vehicle == null) return Results.NotFound();
            return Results.Ok(vehicle);
        })
        .AllowAnonymous()
        .WithName("GetVehicleById")
        .WithSummary("Get vehicle details")
        .Produces<VehicleDetailedViewResponseV1>()
        .Produces(404);

        group.MapGet("search", async ([FromServices] IInventoryService service, [AsParameters] VehicleFilterRequestV1 filter) =>
        {
            IEnumerable<VehicleBasicViewResponseV1> vehicles = await service.SearchVehiclesAsync(filter);
            return Results.Ok(vehicles);
        })
        .AllowAnonymous()
        .WithName("SearchVehicles")
        .WithSummary("Search and filter vehicles")
        .Produces<IEnumerable<VehicleBasicViewResponseV1>>();
    }
}