using AutoDealerPro.Modules.Inventory.Core.Repositories;
using AutoDealerPro.Modules.Inventory.Infrastructure.Persistence;
using AutoDealerPro.Modules.Inventory.Infrastructure.Repositories;
using AutoDealerPro.Shared.Abstractions.Modules;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AutoDealerPro.Modules.Inventory.Infrastructure;

public class InventoryModule : IModule
{
    public string Name => "Inventory";

    public void Register(IServiceCollection services)
    {
        services.AddDbContext<InventoryDbContext>(options =>
            options.UseNpgsql("Host=localhost;Database=autodealerpro;Username=postgres;Password=postgres",
                b => b.MigrationsHistoryTable("__EFMigrationsHistory", "inventory")));

        services.AddScoped<IVehicleRepository, VehicleRepository>();
    }

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapInventoryEndpoints();
    }
}
