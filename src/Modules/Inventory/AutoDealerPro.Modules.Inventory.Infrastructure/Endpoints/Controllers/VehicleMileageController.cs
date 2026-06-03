using AutoDealerPro.Modules.Inventory.Application.Interfaces;
using AutoDealerPro.Modules.Inventory.Application.Requests.UpdateMileage;
using AutoDealerPro.Shared.Abstractions.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoDealerPro.Modules.Inventory.Infrastructure.Endpoints.Controllers;

[ApiController]
[Route("api/vehicle/{id:guid}")]
[Tags("Vehicles")]
[ServiceFilter(typeof(ValidateRequestFilter))]

public class VehicleMileageController(IInventoryService service) : ControllerBase
{
    [HttpPut("mileage")]
    [Authorize(Policy = "StaffOnly")]
    public async Task<IActionResult> UpdateMileage(Guid id, [FromBody] UpdateMileageRequest request)
    {
        try
        {
            await service.UpdateMileageAsync(id, request);
            return NoContent();
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
    }
}