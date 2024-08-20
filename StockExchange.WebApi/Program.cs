using System.Reflection;

using StockExchange.Base.DependencyInjection.Extensions;
using StockExchange.Base.Serialization.Repositories.Interfaces;
using StockExchange.Base.Serialization.Services.Interfaces;
using StockExchange.Logic.Factories.Interfaces;
using StockExchange.Logic.Repositories.Interfaces;
using StockExchange.Logic.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// setup our DI
builder.Services.AddLogging(cfg => cfg.AddConsole());

// add StockExchange services from StockExchange.Base and StockExchange.Services using reflection
builder.Services.AddServicesFromAssembly(Assembly.GetAssembly(typeof(ISerializationService<>)));
builder.Services.AddServicesFromAssembly(Assembly.GetAssembly(typeof(IOrderFactory)));
builder.Services.AddServicesFromAssembly(Assembly.GetAssembly(typeof(ISerializableRepository<>)));
builder.Services.AddServicesFromAssembly(Assembly.GetAssembly(typeof(IBrokerRepository)));
builder.Services.AddServicesFromAssembly(Assembly.GetAssembly(typeof(IPricingService)));

// instantiate depenedency injection concrete object
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
