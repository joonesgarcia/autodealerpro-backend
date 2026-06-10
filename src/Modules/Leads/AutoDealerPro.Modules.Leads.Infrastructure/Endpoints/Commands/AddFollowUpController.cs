using AutoDealerPro.Modules.Leads.Application.V1.Commands.AddFollowUp;
using AutoDealerPro.Modules.Leads.Application.V1.Commands.AddFollowUp.Request;
using AutoDealerPro.Shared.Abstractions.Filter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoDealerPro.Modules.Leads.Infrastructure.Endpoints.Commands;

[ApiController]
[Route("api/v1/leads/{id:guid}/followup")]
[Tags("Leads")]
[Authorize(Policy = "StaffOnly")]
[ServiceFilter(typeof(ValidateRequestFilter))]
public class AddFollowUpController(IMediator mediator) : ControllerBase
{
    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddFollowUp(Guid id, [FromBody] AddFollowUpRequestV1 request, CancellationToken ct)
    {
        var command = new AddFollowUpV1Command(id, request);
        await mediator.Send(command, ct);
        return NoContent();
    }
}