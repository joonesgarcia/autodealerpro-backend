using Microsoft.AspNetCore.Builder;

namespace AutoDealerPro.Shared.Abstractions.Modules;

public static class ModuleExtensions
{
    public static void MapEndpoints(this WebApplication app, List<IModule> modules)
    {
        foreach (IModule module in modules)
        {
            module.MapEndpoints(app);
        }
    }
}
