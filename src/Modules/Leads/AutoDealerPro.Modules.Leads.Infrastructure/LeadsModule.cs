using AutoDealerPro.Modules.Leads.Core.Repositories;
using AutoDealerPro.Modules.Leads.Infrastructure.Persistence;
using AutoDealerPro.Modules.Leads.Infrastructure.Repositories;
using AutoDealerPro.Shared.Abstractions.Modules;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AutoDealerPro.Modules.Leads.Infrastructure;

public class LeadsModule : IModule
{
    public string Name => "Leads";

    public void Register(IServiceCollection services)
    {
        services.AddDbContext<LeadsDbContext>(options =>
            options.UseNpgsql("Host=localhost;Database=autodealerpro;Username=postgres;Password=postgres",
                b => b.MigrationsHistoryTable("__EFMigrationsHistory", "leads")));

        services.AddScoped<ILeadRepository, LeadRepository>();
    }

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapLeadsEndpoints();
    }
}
