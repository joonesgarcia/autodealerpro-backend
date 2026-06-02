using AutoDealerPro.Modules.Auth.Infrastructure;
using AutoDealerPro.Modules.Inventory.Infrastructure;
using AutoDealerPro.Modules.Inventory.Infrastructure.Persistence;
using AutoDealerPro.Modules.Leads.Infrastructure;
using AutoDealerPro.Modules.Leads.Infrastructure.Persistence;
using AutoDealerPro.Shared.Abstractions.Events;
using AutoDealerPro.Shared.Abstractions.Filter;
using AutoDealerPro.Shared.Abstractions.Modules;
using AutoDealerPro.Shared.Infrastructure.Events;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

#region ::: Swagger :::
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AutoDealerPro", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token (without the 'Bearer ' prefix)"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
#endregion

#region ::: Events :::
// Intra-modules communication
// Replace for RabbitMqEventDispatcher/AzureServiceBusEventDispatcher in future
builder.Services.AddScoped<IEventDispatcher, InProcessEventDispatcher>();
#endregion

builder.Services.AddControllers();
builder.Services.AddScoped<ValidateRequestFilter>();

#region ::: Modules :::
List<IModule> modules =
[
    new InventoryModule(),
    new LeadsModule(),
    new AuthModule()
];
modules.ForEach(module => module.Register(builder.Services, builder.Configuration));
#endregion

#region ::: Authentication :::
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["ApiSettings:Issuer"],
            ValidAudience = builder.Configuration["ApiSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["ApiSettings:Secret"] ?? "")),
            ClockSkew = TimeSpan.FromMinutes(5)
        };
    });
#endregion

#region ::: Authorization :::
AuthorizationPolicy requireAuthPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("StaffOnly", policy =>
        policy.RequireRole("Staff", "Admin"))
    .SetFallbackPolicy(requireAuthPolicy);
#endregion

WebApplication app = builder.Build();

using IServiceScope scope = app.Services.CreateScope();

// Applies migrations for both contexts, this is for helping set up the demo dev environment ONLY
// NOT meant to be executed in production

// In production, use proper migration tools or CI/CD pipelines to handle database migrations
// A connection string with this kind of powers should not be used

InventoryDbContext inventoryDb = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
inventoryDb.Database.Migrate();

LeadsDbContext leadsDb = scope.ServiceProvider.GetRequiredService<LeadsDbContext>();
leadsDb.Database.Migrate();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapEndpoints(modules);

app.Run();

public partial class Program { }

