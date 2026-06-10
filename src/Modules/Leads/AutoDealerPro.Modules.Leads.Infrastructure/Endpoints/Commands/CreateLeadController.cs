using AutoDealerPro.Modules.Leads.Application.V1.Commands.CreateLead;
using AutoDealerPro.Modules.Leads.Application.V1.Response;
using AutoDealerPro.Shared.Abstractions.Filter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoDealerPro.Modules.Leads.Infrastructure.Endpoints.Commands;

[ApiController]
[Route("api/v1/leads")]
[Tags("Leads")]
[Authorize(Policy = "StaffOnly")]
[ServiceFilter(typeof(ValidateRequestFilter))]
public class CreateLeadController(IMediator mediator) : ControllerBase
{
    [HttpPost("")]
    [ProducesResponseType(typeof(LeadDetailResponseV1), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateLead([FromBody] CreateLeadV1Command command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return Created($"/api/leads/{result.Id}", result);
    }
}