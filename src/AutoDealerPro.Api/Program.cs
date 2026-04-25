using AutoDealerPro.Modules.Auth.Infrastructure;
using AutoDealerPro.Modules.Inventory.Infrastructure;
using AutoDealerPro.Modules.Inventory.Infrastructure.Persistence;
using AutoDealerPro.Modules.Leads.Infrastructure;
using AutoDealerPro.Modules.Leads.Infrastructure.Persistence;
using AutoDealerPro.Shared.Abstractions.Modules;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

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

#region ::: Modules :::
var modules = new List<IModule>
{
    new InventoryModule(),
    new LeadsModule(),
    new AuthModule()
};
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
var requireAuthPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("StaffOnly", policy =>
        policy.RequireRole("Staff", "Admin"))
    .SetFallbackPolicy(requireAuthPolicy);
#endregion

var app = builder.Build();

using var scope = app.Services.CreateScope();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

logger.LogInformation("Running migrations...");

var inventoryDb = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
inventoryDb.Database.Migrate();
logger.LogInformation("Inventory migrations done.");

var leadsDb = scope.ServiceProvider.GetRequiredService<LeadsDbContext>();
leadsDb.Database.Migrate();
logger.LogInformation("Leads migrations done.");

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapEndpoints(modules);

app.Run();

public partial class Program { }

