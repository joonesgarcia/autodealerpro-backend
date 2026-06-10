using AutoDealerPro.Modules.Leads.Application.V1.Commands.AddFollowUp.Request;
using MediatR;

namespace AutoDealerPro.Modules.Leads.Application.V1.Commands.AddFollowUp;

public record AddFollowUpV1Command(Guid LeadId, AddFollowUpRequestV1 Request) : IRequest<Unit>;
