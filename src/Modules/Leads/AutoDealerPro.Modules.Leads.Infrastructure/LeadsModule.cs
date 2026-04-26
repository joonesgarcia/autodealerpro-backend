using AutoDealerPro.Modules.Inventory.Core.Events;
using AutoDealerPro.Modules.Leads.Application.Interfaces;
using AutoDealerPro.Modules.Leads.Application.Requests;
using AutoDealerPro.Modules.Leads.Application.Services;
using AutoDealerPro.Modules.Leads.Application.Validators;
using AutoDealerPro.Modules.Leads.Core.Repositories;
using AutoDealerPro.Modules.Leads.Infrastructure.EventHandlers;
using AutoDealerPro.Modules.Leads.Infrastructure.Persistence;
using AutoDealerPro.Modules.Leads.Infrastructure.Repositories;
using AutoDealerPro.Shared.Abstractions.Events;
using AutoDealerPro.Shared.Abstractions.Modules;
using FluentValidation;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        services.AddScoped<IValidator<CreateLeadRequest>, CreateLeadValidator>();
        services.AddScoped<IValidator<AssignLeadRequest>, AssignLeadValidator>();
        services.AddScoped<IValidator<MarkAsContactedRequest>, MarkAsContactedValidator>();
        services.AddScoped<IValidator<AddFollowUpRequest>, AddFollowUpValidator>();
    }

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapLeadsEndpoints();
    }
}
