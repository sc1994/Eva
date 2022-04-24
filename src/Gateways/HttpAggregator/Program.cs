using Dapr.Client;
using Eva.HttpAggregator.ServiceInterfaces;
using Eva.HttpAggregator.Services.DemandsServices;
using Eva.ToolKit;

var builder = WebApplication.CreateBuilder(args);

builder.AddCustomAgileConfig();
builder.AddCustomAutoMapper();
builder.AddCustomSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IDemandsService>(_ => new DemandsService(DaprClient.CreateInvokeHttpClient("demands")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();