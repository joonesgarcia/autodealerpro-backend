using AutoDealerPro.Modules.Leads.Application.Exceptions;
using AutoDealerPro.Modules.Leads.Core.Repositories;
using MediatR;

namespace AutoDealerPro.Modules.Leads.Application.V1.Commands.AssignLead;

public class AssignLeadV1CommandHandler(ILeadRepository repository) : IRequestHandler<AssignLeadV1Command, Unit>
{
    private readonly ILeadRepository _repository = repository;

    public async Task<Unit> Handle(AssignLeadV1Command request, CancellationToken cancellationToken)
    {
        var lead = await _repository.GetByIdAsync(request.LeadId, cancellationToken) ??
            throw new LeadNotFoundException(request.LeadId);

        lead.AssignToStaff(request.Request.StaffId);
        await _repository.UpdateAsync(lead, cancellationToken);
        return Unit.Value;
    }
}
