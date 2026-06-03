using AutoDealerPro.Modules.Leads.Application.Interfaces;
using AutoDealerPro.Modules.Leads.Application.Requests.V1;
using AutoDealerPro.Shared.Abstractions.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoDealerPro.Modules.Leads.Infrastructure.Endpoints.Controllers;

[ApiController]
[Route("api/v1/leads/{id:guid}/close")]
[Tags("Leads")]
[Authorize(Policy = "StaffOnly")]
[ServiceFilter(typeof(ValidateRequestFilter))]
public class CloseLeadController(ILeadsService service) : ControllerBase
{
    [HttpPut("")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CloseLead(Guid id, [FromBody] CloseLeadRequestv1 request)
    {
        await service.CloseLeadAsync(id, request);
        return NoContent();
    }
}