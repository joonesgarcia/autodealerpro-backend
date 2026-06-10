using AutoDealerPro.Modules.Leads.Application.V1.Commands.MarkAsContacted;
using AutoDealerPro.Modules.Leads.Application.V1.Commands.MarkAsContacted.Request;
using AutoDealerPro.Shared.Abstractions.Filter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoDealerPro.Modules.Leads.Infrastructure.Endpoints.Commands;

[ApiController]
[Route("api/v1/leads/{id:guid}/contact")]
[Tags("Leads")]
[Authorize(Policy = "StaffOnly")]
[ServiceFilter(typeof(ValidateRequestFilter))]
public class MarkLeadAsContactedController(IMediator mediator) : ControllerBase
{
    [HttpPut("")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkLeadAsContacted(Guid id, [FromBody] MarkAsContactedRequestV1 request, CancellationToken ct)
    {
        var command = new MarkAsContactedV1Command(id, request);
        await mediator.Send(command, ct);
        return NoContent();
    }
}