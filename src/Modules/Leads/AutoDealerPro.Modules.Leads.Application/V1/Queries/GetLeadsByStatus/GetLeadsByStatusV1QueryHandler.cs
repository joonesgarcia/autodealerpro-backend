using AutoDealerPro.Modules.Leads.Application.V1.Extensions;
using AutoDealerPro.Modules.Leads.Application.V1.Response;
using AutoDealerPro.Modules.Leads.Core.Enums;
using AutoDealerPro.Modules.Leads.Core.Repositories;
using MediatR;

namespace AutoDealerPro.Modules.Leads.Application.V1.Queries.GetLeadsByStatus;

public class GetLeadsByStatusV1QueryHandler(ILeadRepository repository) : IRequestHandler<GetLeadsByStatusV1Query, IEnumerable<LeadListResponseV1>>
{
    private readonly ILeadRepository _repository = repository;

    public async Task<IEnumerable<LeadListResponseV1>> Handle(GetLeadsByStatusV1Query request, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse(request.Status, true, out LeadStatus leadStatus))
            throw new ArgumentException($"Invalid lead status: {request.Status}");

        var leads = await _repository.GetByStatusAsync(leadStatus, cancellationToken, request.Page, request.PageSize);
        return leads.Select(x => x.ToListResponse());
    }
}