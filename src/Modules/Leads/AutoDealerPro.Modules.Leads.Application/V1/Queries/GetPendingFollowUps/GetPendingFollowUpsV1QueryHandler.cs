using AutoDealerPro.Modules.Leads.Application.V1.Extensions;
using AutoDealerPro.Modules.Leads.Application.V1.Response;
using AutoDealerPro.Modules.Leads.Core.Repositories;
using MediatR;

namespace AutoDealerPro.Modules.Leads.Application.V1.Queries.GetPendingFollowUps;

public class GetPendingFollowUpsV1QueryHandler(ILeadRepository repository) : IRequestHandler<GetPendingFollowUpsV1Query, IEnumerable<LeadListResponseV1>>
{
    private readonly ILeadRepository _repository = repository;

    public async Task<IEnumerable<LeadListResponseV1>> Handle(GetPendingFollowUpsV1Query request, CancellationToken cancellationToken)
    {
        var leads = await _repository.GetPendingFollowUpsAsync(cancellationToken, request.Page, request.PageSize);
        return leads.Select(x => x.ToListResponse());
    }
}
