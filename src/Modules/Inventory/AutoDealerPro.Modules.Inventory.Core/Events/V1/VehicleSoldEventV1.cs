namespace AutoDealerPro.Modules.Inventory.Core.Events.V1;

using AutoDealerPro.Shared.Abstractions.Events;

// This record is the public contract of the Inventory module.
// Any module that needs to react to a vehicle sale depends on this type only —
// not on any Inventory service, repository, or DbContext.
public record VehicleSoldEventV1(
    Guid VehicleId,
    decimal SellingPrice,
    DateTime OccurredAt) : IDomainEvent
{
    // Convenience constructor — OccurredAt defaults to now
    public VehicleSoldEventV1(Guid vehicleId, decimal sellingPrice)
        : this(vehicleId, sellingPrice, DateTime.UtcNow) { }
}