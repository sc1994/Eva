using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.AddCustomAgileConfig();
builder.AddCustomSerilog();
builder.AddCustomAutoMapper();
builder.AddCustomHealthChecks();

builder.Services.AddSingleton<IFreeSql>(_ =>
{
    var freeSql = new FreeSql.FreeSqlBuilder()
        .UseConnectionString(FreeSql.DataType.MySql, builder.Configuration["DatabaseConnection"])
        .UseAutoSyncStructure(true)
        .Build();

    return freeSql;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDaprClient();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapGet("/", () => Results.LocalRedirect("~/swagger"));
}

app.MapControllers();
app.MapSubscribeHandler();

app.UseCustomHealthChecks();

app.Run();