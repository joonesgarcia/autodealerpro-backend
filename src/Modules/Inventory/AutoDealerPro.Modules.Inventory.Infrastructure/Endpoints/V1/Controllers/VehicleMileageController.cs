using AutoDealerPro.Modules.Inventory.Application.Interfaces;
using AutoDealerPro.Modules.Inventory.Application.Requests.V1.UpdateMileage;
using AutoDealerPro.Shared.Abstractions.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoDealerPro.Modules.Inventory.Infrastructure.Endpoints.V1.Controllers;

[ApiController]
[Route("api/v1/vehicle/{id:guid}")]
[Tags("Vehicles")]
[ServiceFilter(typeof(ValidateRequestFilter))]

public class VehicleMileageController(IInventoryService service) : ControllerBase
{
    [HttpPut("mileage")]
    [Authorize(Policy = "StaffOnly")]
    public async Task<IActionResult> UpdateMileage(Guid id, [FromBody] UpdateMileageRequestV1 request)
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