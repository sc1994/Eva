using Eva.ProxyAggregator.Options;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.AddCustomAgileConfig();
builder.AddCustomAutoMapper();
builder.AddCustomSerilog();
builder.AddCustomHealthChecks();

builder.Services.AddDaprClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.Configure<ProxySetting>(builder.Configuration.GetSection("ProxySetting"));
builder.Services.AddScoped<ProxySetting>(provider =>
{
    try
    {
        var json = builder.Configuration["ProxySetting"];

        return JsonConvert.DeserializeObject<ProxySetting>(json) ?? new ProxySetting();
    }
    catch (Exception e)
    {
        provider.GetRequiredService<ILogger<Program>>().LogError(e, "Failed to load proxy settings");
        return new ProxySetting();
    }
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapGet("/", () => Results.LocalRedirect("~/swagger"));
    
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "聚合网关 V1");

        using var scope = app.Services.CreateScope();
        var proxyConfig = scope.ServiceProvider.GetRequiredService<ProxySetting>();

        if (proxyConfig.Services?.Any() != true)
            return;

        foreach (var service in proxyConfig.Services)
        {
            options.SwaggerEndpoint($"/swagger/{service.Appid}/v1/swagger.json", $"{service.Description} V1");
        }
    });
}

app.MapControllers();
app.MapSubscribeHandler();

app.MapHealthChecks("/hc", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("/liveness", new HealthCheckOptions
{
    Predicate = r => r.Name.Contains("self")
});

app.Run();