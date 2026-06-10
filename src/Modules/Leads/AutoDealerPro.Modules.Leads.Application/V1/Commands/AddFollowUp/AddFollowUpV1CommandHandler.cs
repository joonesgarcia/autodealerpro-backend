using AutoDealerPro.Modules.Leads.Application.Exceptions;
using AutoDealerPro.Modules.Leads.Core.Repositories;
using MediatR;

namespace AutoDealerPro.Modules.Leads.Application.V1.Commands.AddFollowUp;

public class AddFollowUpV1CommandHandler(ILeadRepository repository) : IRequestHandler<AddFollowUpV1Command, Unit>
{
    private readonly ILeadRepository _repository = repository;

    public async Task<Unit> Handle(AddFollowUpV1Command request, CancellationToken cancellationToken)
    {
        var lead = await _repository.GetByIdAsync(request.LeadId, cancellationToken) ??
            throw new LeadNotFoundException(request.LeadId);

        lead.AddFollowUp(request.Request.Note, request.Request.NextFollowUpDate);
        await _repository.UpdateAsync(lead, cancellationToken);
        return Unit.Value;
    }
}
