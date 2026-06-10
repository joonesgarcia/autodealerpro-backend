using AutoDealerPro.Modules.Leads.Application.V1.Commands.CloseLead.Request;
using MediatR;

namespace AutoDealerPro.Modules.Leads.Application.V1.Commands.CloseLead;

public record CloseLeadV1Command(Guid LeadId, CloseLeadRequestv1 Request) : IRequest<Unit>;
