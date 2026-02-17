using Microsoft.Extensions.DependencyInjection;

namespace AutoDealerPro.Shared.Abstractions.Modules;

public interface IModule
{
    string Name { get; }
    void Register(IServiceCollection services);
}