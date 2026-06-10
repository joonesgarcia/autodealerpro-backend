using AutoDealerPro.Modules.Leads.Application.V1.Commands.MarkAsContacted.Request;
using MediatR;

namespace AutoDealerPro.Modules.Leads.Application.V1.Commands.MarkAsContacted;

public record MarkAsContactedV1Command(Guid LeadId, MarkAsContactedRequestV1 Request) : IRequest<Unit>;
