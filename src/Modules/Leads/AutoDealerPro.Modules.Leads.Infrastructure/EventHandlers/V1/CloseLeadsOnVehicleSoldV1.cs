using AutoDealerPro.Modules.Inventory.Core.Events.V1;
using AutoDealerPro.Modules.Leads.Core.Enums;
using AutoDealerPro.Modules.Leads.Core.Repositories;
using AutoDealerPro.Shared.Abstractions.Events;

namespace AutoDealerPro.Modules.Leads.Infrastructure.EventHandlers.V1;

public class CloseLeadsOnVehicleSoldV1(ILeadRepository leadRepository)
    : IDomainEventHandler<VehicleSoldEventV1>
{
    public async Task Handle(VehicleSoldEventV1 @event, CancellationToken ct = default)
    {
        IEnumerable<Core.Entities.Lead> leads = await leadRepository.GetByVehicleIdAsync(@event.VehicleId);

        IEnumerable<Core.Entities.Lead> openLeads = leads.Where(l =>
            l.Status is not LeadStatus.Converted and not LeadStatus.Lost);

        foreach (Core.Entities.Lead? lead in openLeads)
        {
            lead.MarkAsClosed(converted: false);
            await leadRepository.UpdateAsync(lead);
        }
    }
}
