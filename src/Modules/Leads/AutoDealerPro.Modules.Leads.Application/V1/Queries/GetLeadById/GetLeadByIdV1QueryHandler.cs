using AutoDealerPro.Modules.Leads.Application.Exceptions;
using AutoDealerPro.Modules.Leads.Application.V1.Extensions;
using AutoDealerPro.Modules.Leads.Application.V1.Response;
using AutoDealerPro.Modules.Leads.Core.Entities;
using AutoDealerPro.Modules.Leads.Core.Repositories;
using MediatR;

namespace AutoDealerPro.Modules.Leads.Application.V1.Queries.GetLeadById;

internal class GetLeadByIdV1QueryHandler(ILeadRepository repository) : IRequestHandler<GetLeadByIdV1Query, LeadDetailResponseV1>
{
    private readonly ILeadRepository _repository = repository;

    public async Task<LeadDetailResponseV1> Handle(GetLeadByIdV1Query request, CancellationToken cancellationToken)
    {
        Lead? lead = await _repository.GetByIdAsync(request.LeadId, cancellationToken) ??
            throw new LeadNotFoundException(request.LeadId);
        return lead.ToDetailResponse();
    }
}
