namespace AutoDealerPro.Shared.Abstractions.Events;

public interface IEventDispatcher
{
    Task Publish<T>(T @event, CancellationToken ct = default) where T : IDomainEvent;
}
