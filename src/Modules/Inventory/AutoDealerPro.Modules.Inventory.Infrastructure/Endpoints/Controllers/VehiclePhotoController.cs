using AutoDealerPro.Modules.Inventory.Application.Interfaces;
using AutoDealerPro.Modules.Inventory.Application.Requests.AddPhoto;
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

public class VehiclePhotoController(IInventoryService service) : ControllerBase
{
    [HttpPost("photos")]
    [Authorize(Policy = "StaffOnly")]
    public async Task<IActionResult> AddPhoto(Guid id, [FromBody] AddPhotoRequest request)
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