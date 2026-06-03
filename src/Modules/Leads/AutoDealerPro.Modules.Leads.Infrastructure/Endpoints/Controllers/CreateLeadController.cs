using AutoDealerPro.Modules.Leads.Application.Interfaces;
using AutoDealerPro.Modules.Leads.Application.Requests;
using AutoDealerPro.Modules.Leads.Application.Response;
using AutoDealerPro.Shared.Abstractions.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoDealerPro.Modules.Leads.Infrastructure.Endpoints.Controllers;

[ApiController]
[Route("api/leads")]
[Tags("Leads")]
[Authorize(Policy = "StaffOnly")]
[ServiceFilter(typeof(ValidateRequestFilter))]
public class CreateLeadController(ILeadsService service) : ControllerBase
{
    [HttpPost("")]
    [ProducesResponseType(typeof(LeadDetailResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateLead([FromBody] CreateLeadRequest request)
    {
        LeadDetailResponse result = await service.CreateLeadAsync(request);
        return Created($"/api/leads/{result.Id}", result);
    }
}