using AutoDealerPro.Modules.Leads.Application.Exceptions;
using AutoDealerPro.Modules.Leads.Core.Repositories;
using MediatR;

namespace AutoDealerPro.Modules.Leads.Application.V1.Commands.MarkAsContacted;

public class MarkAsContactedV1CommandHandler(ILeadRepository repository) : IRequestHandler<MarkAsContactedV1Command, Unit>
{
    private readonly ILeadRepository _repository = repository;

    public async Task<Unit> Handle(MarkAsContactedV1Command request, CancellationToken cancellationToken)
    {
        var lead = await _repository.GetByIdAsync(request.LeadId, cancellationToken) ??
            throw new LeadNotFoundException(request.LeadId);

        lead.MarkAsContacted(request.Request.Notes);
        await _repository.UpdateAsync(lead, cancellationToken);
        return Unit.Value;
    }
}
