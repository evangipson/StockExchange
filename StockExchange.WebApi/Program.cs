using System.Reflection;
using StockExchange.Base.DependencyInjection;
using StockExchange.Logic.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// setup our DI
builder.Services.AddLogging(cfg => cfg.AddConsole());

// add StockExchange services from StockExchange.Services using reflection
builder.Services.AddServicesFromAssembly(Assembly.GetAssembly(typeof(ISerializationService)));

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
