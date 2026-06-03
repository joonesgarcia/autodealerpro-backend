using AutoDealerPro.Modules.Inventory.Application.Interfaces;
using AutoDealerPro.Modules.Inventory.Application.Requests.V1.AddPhoto;
using AutoDealerPro.Shared.Abstractions.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoDealerPro.Modules.Inventory.Infrastructure.Endpoints.V1.Controllers;

[ApiController]
[Route("api/v1/vehicle/{id:guid}")]
[Tags("Vehicles")]
[ServiceFilter(typeof(ValidateRequestFilter))]

public class VehiclePhotoController(IInventoryService service) : ControllerBase
{
    [HttpPost("photos")]
    [Authorize(Policy = "StaffOnly")]
    public async Task<IActionResult> AddPhoto(Guid id, [FromBody] AddPhotoRequestV1 request)
    {
        try
        {
            await service.AddPhotoAsync(id, request);
            return NoContent();
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
    }
}