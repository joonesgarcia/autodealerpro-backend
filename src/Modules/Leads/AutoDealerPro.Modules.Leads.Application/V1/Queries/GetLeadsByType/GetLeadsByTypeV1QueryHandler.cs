using AutoDealerPro.Modules.Leads.Application.V1.Extensions;
using AutoDealerPro.Modules.Leads.Application.V1.Response;
using AutoDealerPro.Modules.Leads.Core.Enums;
using AutoDealerPro.Modules.Leads.Core.Repositories;
using MediatR;

namespace AutoDealerPro.Modules.Leads.Application.V1.Queries.GetLeadsByType;

public class GetLeadsByTypeV1QueryHandler(ILeadRepository repository) : IRequestHandler<GetLeadsByTypeV1Query, IEnumerable<LeadListResponseV1>>
{
    private readonly ILeadRepository _repository = repository;

    public async Task<IEnumerable<LeadListResponseV1>> Handle(GetLeadsByTypeV1Query request, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse(request.Type, true, out LeadType leadType))
            throw new ArgumentException($"Invalid lead type: {request.Type}");

        var leads = await _repository.GetByTypeAsync(leadType, cancellationToken, request.Page, request.PageSize);
        return leads.Select(x => x.ToListResponse());
    }
}
