using AutoDealerPro.Modules.Inventory.Application.Interfaces;
using AutoDealerPro.Modules.Inventory.Application.Requests.MarkAsSold;
using AutoDealerPro.Shared.Abstractions.Filter;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoDealerPro.Modules.Inventory.Infrastructure.Endpoints.Controllers;

[ApiController]
[Route("api/vehicle/{id:guid}")]
[Tags("Vehicles")]
[ServiceFilter(typeof(ValidateRequestFilter))]

public class VehicleSalesController(IInventoryService service) : ControllerBase
{
    [HttpPost("mark-sold")]
    [Authorize(Policy = "StaffOnly")]
    public async Task<IActionResult> MarkAsSold(Guid id, [FromBody] MarkAsSoldRequest request)
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