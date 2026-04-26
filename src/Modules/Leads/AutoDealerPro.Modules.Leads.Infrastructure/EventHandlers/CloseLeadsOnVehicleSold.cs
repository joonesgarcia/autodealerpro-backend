using AutoDealerPro.Modules.Inventory.Core.Events;
using AutoDealerPro.Modules.Leads.Core.Enums;
using AutoDealerPro.Modules.Leads.Core.Repositories;
using AutoDealerPro.Shared.Abstractions.Events;

namespace AutoDealerPro.Modules.Leads.Infrastructure.EventHandlers;

public class CloseLeadsOnVehicleSold(ILeadRepository leadRepository)
    : IDomainEventHandler<VehicleSoldEvent>
{
    public async Task Handle(VehicleSoldEvent @event, CancellationToken ct = default)
    {
        var leads = await leadRepository.GetByVehicleIdAsync(@event.VehicleId);

        var openLeads = leads.Where(l =>
            l.Status is not LeadStatus.Converted and not LeadStatus.Lost);

        foreach (var lead in openLeads)
        {
            lead.MarkAsClosed(converted: false);
            await leadRepository.UpdateAsync(lead);
        }
    }
}
