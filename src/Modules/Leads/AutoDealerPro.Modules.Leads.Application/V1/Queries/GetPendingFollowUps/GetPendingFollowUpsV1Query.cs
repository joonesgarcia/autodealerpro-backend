using AutoDealerPro.Modules.Leads.Application.V1.Response;
using MediatR;

namespace AutoDealerPro.Modules.Leads.Application.V1.Queries.GetPendingFollowUps;

public record GetPendingFollowUpsV1Query(int Page = 1, int PageSize = 10) : IRequest<IEnumerable<LeadListResponseV1>>;
