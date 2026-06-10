using AutoDealerPro.Modules.Inventory.Core.Events.V1;
using AutoDealerPro.Modules.Leads.Core.Entities;
using AutoDealerPro.Modules.Leads.Core.Enums;
using AutoDealerPro.Modules.Leads.Core.Repositories;
using AutoDealerPro.Shared.Abstractions.Events;

namespace AutoDealerPro.Modules.Leads.Infrastructure.EventHandlers.V1;

// This needs to be changed to outbox pattern in the future.
// If inventory wants to notify leads module about a vehicle being sold,
// it should publish an event to the outbox, and then a separate process should read that event and handle it.
public class CloseLeadsOnVehicleSoldV1(ILeadRepository leadRepository)
    : IDomainEventHandler<VehicleSoldEventV1>
{
    public async Task Handle(VehicleSoldEventV1 @event, CancellationToken ct = default)
    {
        IEnumerable<Lead> leads = await leadRepository.GetByVehicleIdAsync(@event.VehicleId, ct);

        IEnumerable<Lead> openLeads = leads.Where(l =>
            l.Status is not LeadStatus.Converted and not LeadStatus.Lost);

        foreach (Lead? lead in openLeads)
        {
            lead.MarkAsClosed(converted: false);
            await leadRepository.UpdateAsync(lead, ct);
        }
    }
}
