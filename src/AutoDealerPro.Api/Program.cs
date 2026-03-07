using AutoDealerPro.Modules.Inventory.Infrastructure;
using AutoDealerPro.Modules.Leads.Infrastructure;
using AutoDealerPro.Shared.Abstractions.Modules;

var builder = WebApplication.CreateBuilder(args);

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register all modules
var modules = new List<IModule>
{
    new InventoryModule(),
    new LeadsModule()
};

foreach (var module in modules)
{
    module.Register(builder.Services);
}

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();

var app = builder.Build();

foreach (var module in modules)
{
    module.MapEndpoints(app);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();     
    app.UseSwaggerUI(); 
}

app.UseHttpsRedirection();



app.Run();


