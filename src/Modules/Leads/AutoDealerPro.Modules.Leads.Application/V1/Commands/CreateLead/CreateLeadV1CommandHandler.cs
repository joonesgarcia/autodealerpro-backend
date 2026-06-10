using AutoDealerPro.Modules.Leads.Application.Exceptions;
using AutoDealerPro.Modules.Leads.Application.V1.Extensions;
using AutoDealerPro.Modules.Leads.Application.V1.Response;
using AutoDealerPro.Modules.Leads.Core.Entities;
using AutoDealerPro.Modules.Leads.Core.Repositories;
using MediatR;

namespace AutoDealerPro.Modules.Leads.Application.V1.Commands.CreateLead;

public class CreateLeadV1CommandHandler(ILeadRepository repository) : IRequestHandler<CreateLeadV1Command, LeadDetailResponseV1>
{
    private readonly ILeadRepository _repository = repository;

    public async Task<LeadDetailResponseV1> Handle(CreateLeadV1Command request, CancellationToken cancellationToken)
    {
        var existingLead = await _repository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingLead != null)
            throw new DuplicateLeadException(request.Email);

        var lead = Lead.Create(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Phone,
            request.VehicleId,
            request.Type,
            request.Message,
            request.TradeInMake,
            request.TradeInModel,
            request.TradeInYear,
            request.TradeInMileage
        );

        await _repository.AddAsync(lead, cancellationToken);
        return lead.ToDetailResponse();
    }
}
