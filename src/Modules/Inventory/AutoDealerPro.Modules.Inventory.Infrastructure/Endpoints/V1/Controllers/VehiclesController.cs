using AutoDealerPro.Modules.Inventory.Application.Interfaces;
using AutoDealerPro.Modules.Inventory.Application.Requests.V1.AddVehicle;
using AutoDealerPro.Modules.Inventory.Application.Responses.V1;
using AutoDealerPro.Shared.Abstractions.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoDealerPro.Modules.Inventory.Infrastructure.Endpoints.V1.Controllers;

[ApiController]
[Route("api/v1/vehicle")]
[Tags("Vehicles")]
[ServiceFilter(typeof(ValidateRequestFilter))]
public class VehiclesController(IInventoryService service) : ControllerBase
{
    [HttpPost("")]
    [Authorize(Policy = "StaffOnly")]
    public async Task<IActionResult> CreateVehicle([FromBody] AddVehicleRequestV1 request)
    {
        try
        {
            VehicleStaffViewResponseV1 vehicle = await service.CreateVehicleAsync(request);
            return Created($"/api/vehicles/{vehicle.Id}", vehicle);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
    }
}