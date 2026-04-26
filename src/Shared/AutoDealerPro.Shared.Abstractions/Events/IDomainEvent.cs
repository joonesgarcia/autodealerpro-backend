namespace AutoDealerPro.Shared.Abstractions.Events;

public interface IDomainEvent
{
    DateTime OccurredAt { get; }
}
