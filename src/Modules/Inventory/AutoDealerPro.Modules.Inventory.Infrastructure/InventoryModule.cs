using AutoDealerPro.Modules.Inventory.Application.Interfaces;
using AutoDealerPro.Modules.Inventory.Application.Services;
using AutoDealerPro.Modules.Inventory.Core.Repositories;
using AutoDealerPro.Modules.Inventory.Infrastructure.Endpoints;
using AutoDealerPro.Modules.Inventory.Infrastructure.Persistence;
using AutoDealerPro.Modules.Inventory.Infrastructure.Repositories;
using AutoDealerPro.Shared.Abstractions.Modules;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AutoDealerPro.Modules.Inventory.Infrastructure;

public class InventoryModule : IModule
{
    public string Name => "Inventory";

    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<InventoryDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsHistoryTable("__EFMigrationsHistory", "inventory")));

        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IInventoryService, InventoryService>();

        // Validators
        services.AddValidatorsFromAssembly(Assembly.Load("AutoDealerPro.Modules.Inventory.Application"));
    }

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapControllers();
        endpoints.MapVehicleQueryRoutes();

    }
}
