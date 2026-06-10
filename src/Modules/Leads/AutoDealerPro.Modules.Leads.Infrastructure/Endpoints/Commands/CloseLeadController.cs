using AutoDealerPro.Modules.Leads.Application.V1.Commands.CloseLead;
using AutoDealerPro.Modules.Leads.Application.V1.Commands.CloseLead.Request;
using AutoDealerPro.Shared.Abstractions.Filter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoDealerPro.Modules.Leads.Infrastructure.Endpoints.Commands;

[ApiController]
[Route("api/v1/leads/{id:guid}/close")]
[Tags("Leads")]
[Authorize(Policy = "StaffOnly")]
[ServiceFilter(typeof(ValidateRequestFilter))]
public class CloseLeadController(IMediator mediator) : ControllerBase
{
    [HttpPut("")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CloseLead(Guid id, [FromBody] CloseLeadRequestv1 request, CancellationToken ct)
    {
        var command = new CloseLeadV1Command(id, request);
        await mediator.Send(command, ct);
        return NoContent();
    }
}