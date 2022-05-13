using Eva.ToolKit.Middlewares;

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

// 注入
builder.Services.AddScoped<ThrowFriendlyException>();
builder.Services.Configure<ProxySetting>(builder.Configuration.GetSection("ProxySetting"));
builder.Services.AddScoped<ProxySetting>(provider =>
{
    try
    {
        var json = builder.Configuration["ProxySetting"];

        return json.ConvertJsonToObject<ProxySetting>() ?? new ProxySetting();
    }
    catch (Exception e)
    {
        provider.GetRequiredService<ILogger<Program>>().LogError(e, "Failed to load proxy settings");
        return new ProxySetting();
    }
});

var app = builder.Build();

// 开发环境特有的设置
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

        foreach (var service in proxyConfig.Services) options.SwaggerEndpoint($"/swagger/{service.Appid}/v1/swagger.json", $"{service.Description} V1");
    });
}

app.UseMiddleware<ThrowFriendlyException>();

app.MapControllers();
app.MapSubscribeHandler();
app.UseHttpLogging();

app.UseCustomHealthChecks();

app.Run();