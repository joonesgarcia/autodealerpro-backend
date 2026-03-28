using AutoDealerPro.Modules.Inventory.Infrastructure;
using AutoDealerPro.Modules.Leads.Infrastructure;
using AutoDealerPro.Shared.Abstractions.Modules;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region ::: Swagger :::

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

#region ::: Modules :::

var modules = new List<IModule>
{
    new InventoryModule(),
    new LeadsModule()
};

modules.ForEach(module => module.Register(builder.Services));

#endregion

#region ::: Authentication :::

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["ApiSettings:Issuer"],
            ValidAudience = builder.Configuration["ApiSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["ApiSettings:Secret"]))
        };
    });

#region ::: Policies :::

var requireAuthPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("StaffOnly", policy =>
        policy.RequireRole("Staff", "Admin"))
    .SetFallbackPolicy(requireAuthPolicy);

#endregion

#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapEndpoints(modules);

app.Run();


