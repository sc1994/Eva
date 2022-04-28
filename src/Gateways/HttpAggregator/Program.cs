using Eva.HttpAggregator.ServiceInterfaces;
using Eva.HttpAggregator.Services.DemandsServices;
using Eva.HttpAggregator.Services.ProxyServices;

var builder = WebApplication.CreateBuilder(args);

builder.AddCustomAgileConfig();
builder.AddCustomAutoMapper();
builder.AddCustomSerilog();

builder.Services.AddDaprClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<IDemandsService, DemandsService>();
builder.Services.AddSingleton<IProxyService, ProxyService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "聚合网关 V1");

        var proxyConfig = app.Services.GetRequiredService<IProxyService>().GetConfig();
        
        if (proxyConfig?.Services?.Any() != true)
            return;

        foreach (var service in proxyConfig.Services)
        {
            options.SwaggerEndpoint($"/{service.Appid}/swagger/v1/swagger.json", $"{service.Description} V1");
        }
    });
}

app.MapControllers();

app.Run();