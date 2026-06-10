using AutoDealerPro.Modules.Leads.Application.V1.Commands.AssignLead.Request;
using MediatR;

namespace AutoDealerPro.Modules.Leads.Application.V1.Commands.AssignLead;

public record AssignLeadV1Command(Guid LeadId, AssignLeadRequestV1 Request) : IRequest<Unit>;
