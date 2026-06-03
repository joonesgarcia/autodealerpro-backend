using AutoDealerPro.Modules.Inventory.Application.Interfaces;
using AutoDealerPro.Modules.Inventory.Application.Requests.AddVehicle;
using AutoDealerPro.Modules.Inventory.Application.Responses;
using AutoDealerPro.Shared.Abstractions.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoDealerPro.Modules.Inventory.Infrastructure.Endpoints.Controllers;

[ApiController]
[Route("api/vehicle")]
[Tags("Vehicles")]
[ServiceFilter(typeof(ValidateRequestFilter))]
public class VehiclesController(IInventoryService service) : ControllerBase
{
    [HttpPost("")]
    [Authorize(Policy = "StaffOnly")]
    public async Task<IActionResult> CreateVehicle([FromBody] AddVehicleRequest request)
    {
        try
        {
            VehicleStaffViewResponse vehicle = await service.CreateVehicleAsync(request);
            return Created($"/api/vehicles/{vehicle.Id}", vehicle);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
    }
}