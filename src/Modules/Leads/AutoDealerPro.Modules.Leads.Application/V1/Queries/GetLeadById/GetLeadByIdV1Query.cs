using AutoDealerPro.Modules.Leads.Application.V1.Response;
using MediatR;

namespace AutoDealerPro.Modules.Leads.Application.V1.Queries.GetLeadById;

public record GetLeadByIdV1Query(Guid LeadId) : IRequest<LeadDetailResponseV1>;
