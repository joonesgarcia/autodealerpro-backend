using AutoDealerPro.Modules.Leads.Application.V1.Response;
using MediatR;

namespace AutoDealerPro.Modules.Leads.Application.V1.Queries.GetLeadsByStatus;

public record GetLeadsByStatusV1Query(string Status, int Page = 1, int PageSize = 10) : IRequest<IEnumerable<LeadListResponseV1>>;
