using AutoDealerPro.Modules.Leads.Application.Exceptions;
using AutoDealerPro.Modules.Leads.Core.Repositories;
using MediatR;

namespace AutoDealerPro.Modules.Leads.Application.V1.Commands.CloseLead;

public class CloseLeadV1CommandHandler(ILeadRepository repository) : IRequestHandler<CloseLeadV1Command, Unit>
{
    private readonly ILeadRepository _repository = repository;

    public async Task<Unit> Handle(CloseLeadV1Command request, CancellationToken cancellationToken)
    {
        var lead = await _repository.GetByIdAsync(request.LeadId, cancellationToken) ??
            throw new LeadNotFoundException(request.LeadId);

        lead.MarkAsClosed(request.Request.Converted);
        await _repository.UpdateAsync(lead, cancellationToken);
        return Unit.Value;
    }
}
