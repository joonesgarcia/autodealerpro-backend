using AutoDealerPro.Modules.Inventory.Application.Interfaces;
using AutoDealerPro.Modules.Inventory.Application.Requests.V1.MarkAsSold;
using AutoDealerPro.Shared.Abstractions.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoDealerPro.Modules.Inventory.Infrastructure.Endpoints.V1.Controllers;

[ApiController]
[Route("api/v1/vehicle/{id:guid}")]
[Tags("Vehicles")]
[ServiceFilter(typeof(ValidateRequestFilter))]

public class VehicleSalesController(IInventoryService service) : ControllerBase
{
    [HttpPost("mark-sold")]
    [Authorize(Policy = "StaffOnly")]
    public async Task<IActionResult> MarkAsSold(Guid id, [FromBody] MarkAsSoldRequestV1 request)
    {
        try
        {
            await service.MarkAsSoldAsync(id, request);
            return NoContent();
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
    }
}