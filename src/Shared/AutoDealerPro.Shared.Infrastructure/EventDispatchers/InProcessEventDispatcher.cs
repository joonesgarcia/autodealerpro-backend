using AutoDealerPro.Shared.Abstractions.Events;
using Microsoft.Extensions.DependencyInjection;

namespace AutoDealerPro.Shared.Infrastructure.Events;

public class InProcessEventDispatcher(IServiceProvider serviceProvider) : IEventDispatcher
{
    public async Task Publish<T>(T @event, CancellationToken ct = default)
        where T : IDomainEvent
    {
        IEnumerable<IDomainEventHandler<T>> handlers = serviceProvider.GetServices<IDomainEventHandler<T>>();

        foreach (IDomainEventHandler<T> handler in handlers)
            await handler.Handle(@event, ct);
    }
}