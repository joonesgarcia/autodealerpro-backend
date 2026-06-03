using AutoDealerPro.Modules.Leads.Application.Interfaces;
using AutoDealerPro.Modules.Leads.Application.Requests.V1.AddFollowUp;
using AutoDealerPro.Shared.Abstractions.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoDealerPro.Modules.Leads.Infrastructure.Endpoints.Controllers;

[ApiController]
[Route("api/v1/leads/{id:guid}/followup")]
[Tags("Leads")]
[Authorize(Policy = "StaffOnly")]
[ServiceFilter(typeof(ValidateRequestFilter))]
public class AddFollowUpController(ILeadsService service) : ControllerBase
{
    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddFollowUp(Guid id, [FromBody] AddFollowUpRequestV1 request)
    {
        await service.AddFollowUpAsync(id, request);
        return NoContent();
    }
}