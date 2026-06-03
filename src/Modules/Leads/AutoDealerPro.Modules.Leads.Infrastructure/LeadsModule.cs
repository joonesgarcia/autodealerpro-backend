using AutoDealerPro.Modules.Inventory.Core.Events;
using AutoDealerPro.Modules.Leads.Application.Interfaces;
using AutoDealerPro.Modules.Leads.Application.Services;
using AutoDealerPro.Modules.Leads.Core.Repositories;
using AutoDealerPro.Modules.Leads.Infrastructure.Endpoints;
using AutoDealerPro.Modules.Leads.Infrastructure.EventHandlers;
using AutoDealerPro.Modules.Leads.Infrastructure.Persistence;
using AutoDealerPro.Modules.Leads.Infrastructure.Repositories;
using AutoDealerPro.Shared.Abstractions.Events;
using AutoDealerPro.Shared.Abstractions.Modules;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AutoDealerPro.Modules.Leads.Infrastructure;

public class LeadsModule : IModule
{
    public string Name => "Leads";

    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<LeadsDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsHistoryTable("__EFMigrationsHistory", "leads")));

        // Repositories
        services.AddScoped<ILeadRepository, LeadRepository>();

        // Services
        services.AddScoped<ILeadsService, LeadsService>();

        // Event Handlers
        services.AddScoped<IDomainEventHandler<VehicleSoldEvent>, CloseLeadsOnVehicleSold>();

        // Validators
        services.AddValidatorsFromAssembly(Assembly.Load("AutoDealerPro.Modules.Leads.Application"));

    }

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapControllers();
        endpoints.MapLeadsQueryRoutes();
    }
}
