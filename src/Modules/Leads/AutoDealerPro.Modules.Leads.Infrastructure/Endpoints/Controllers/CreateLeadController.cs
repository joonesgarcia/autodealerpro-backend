using AutoDealerPro.Modules.Leads.Application.Interfaces;
using AutoDealerPro.Modules.Leads.Application.Requests.V1.CreateLead;
using AutoDealerPro.Modules.Leads.Application.Response.V1;
using AutoDealerPro.Shared.Abstractions.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoDealerPro.Modules.Leads.Infrastructure.Endpoints.Controllers;

[ApiController]
[Route("api/v1/leads")]
[Tags("Leads")]
[Authorize(Policy = "StaffOnly")]
[ServiceFilter(typeof(ValidateRequestFilter))]
public class CreateLeadController(ILeadsService service) : ControllerBase
{
    [HttpPost("")]
    [ProducesResponseType(typeof(LeadDetailResponseV1), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateLead([FromBody] CreateLeadRequestV1 request)
    {
        LeadDetailResponseV1 result = await service.CreateLeadAsync(request);
        return Created($"/api/leads/{result.Id}", result);
    }
}