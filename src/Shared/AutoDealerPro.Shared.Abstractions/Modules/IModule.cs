using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace AutoDealerPro.Shared.Abstractions.Modules;

public interface IModule
{
    string Name { get; }

    void Register(IServiceCollection services);

    void MapEndpoints(IEndpointRouteBuilder endpoints);
}