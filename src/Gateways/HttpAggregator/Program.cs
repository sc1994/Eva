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

builder.Services.AddSingleton<IDemandsService>(_ => new DemandsService(DaprClient.CreateInvokeHttpClient("demands")));
builder.Services.AddSingleton<IProxyService, ProxyService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    var proxyConfig = app.Services.GetRequiredService<IProxyService>().GetConfig();

    app.MapGet("/{appid}/swagger/v1/swagger.json", async ctx =>
    {
        var appId = ctx.Request.RouteValues["appid"]?.ToString();
        if (string.IsNullOrWhiteSpace(appId))
            throw new ArgumentNullException(nameof(appId));

        var response = await ctx.RequestServices.GetRequiredService<IProxyService>().TryProxySwaggerJsonAsync(ctx);

        var swagger = JsonConvert.DeserializeObject<JObject>(response);

        var tmp = new List<JProperty>();
        foreach (var jToken in swagger?["paths"] ?? throw new NullReferenceException("swagger?[\"paths\"]"))
        {
            var item = (JProperty) jToken;
            tmp.Add(item);
        }

        foreach (var item in tmp)
        {
            item.Replace(new JProperty($"/{appId}{item.Name}", item.Value));
        }

        await ctx.Response.WriteAsync(swagger.ToString());
    });

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "聚合网关 V1");

        if (proxyConfig?.Services?.Any() != true)
            return;

        foreach (var service in proxyConfig.Services)
        {
            options.SwaggerEndpoint($"/{service.Appid}/swagger/v1/swagger.json", $"{service.Description} V1");
        }
    });
}

app.Use(async (ctx, next) =>
{
    if (await ctx.RequestServices.GetRequiredService<IProxyService>().TryProxyAsync(ctx))
        return;

    await next();
});

app.MapControllers();

app.Run();