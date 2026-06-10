using AutoDealerPro.Modules.Leads.Application.V1.Extensions;
using AutoDealerPro.Modules.Leads.Application.V1.Response;
using AutoDealerPro.Modules.Leads.Core.Repositories;
using MediatR;

namespace AutoDealerPro.Modules.Leads.Application.V1.Queries.GetLeadsByVehicleIdQuery;

public class GetLeadsByVehicleIdV1QueryHandler(ILeadRepository repository) : IRequestHandler<GetLeadsByVehicleIdV1Query, IEnumerable<LeadListResponseV1>>
{
    private readonly ILeadRepository _repository = repository;

    public async Task<IEnumerable<LeadListResponseV1>> Handle(GetLeadsByVehicleIdV1Query request, CancellationToken cancellationToken)
    {
        var leads = await _repository.GetByVehicleIdAsync(request.VehicleId, cancellationToken);
        return leads.Select(x => x.ToListResponse());
    }
}
