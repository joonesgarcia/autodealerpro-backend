using AutoDealerPro.Modules.Inventory.Core.Events.V1;
using AutoDealerPro.Modules.Leads.Core.Entities;
using AutoDealerPro.Modules.Leads.Core.Enums;
using AutoDealerPro.Modules.Leads.Core.Repositories;
using AutoDealerPro.Shared.Abstractions.Events;

namespace AutoDealerPro.Modules.Leads.Infrastructure.EventHandlers.V1;

public class CloseLeadsOnVehicleSoldV1(ILeadRepository leadRepository)
    : IDomainEventHandler<VehicleSoldEventV1>
{
    public async Task Handle(VehicleSoldEventV1 @event, CancellationToken ct = default)
    {
        IEnumerable<Lead> leads = await leadRepository.GetByVehicleIdAsync(@event.VehicleId);

        IEnumerable<Lead> openLeads = leads.Where(l =>
            l.Status is not LeadStatus.Converted and not LeadStatus.Lost);

        foreach (Lead? lead in openLeads)
        {
            lead.MarkAsClosed(converted: false);
            await leadRepository.UpdateAsync(lead);
        }
    }
}
