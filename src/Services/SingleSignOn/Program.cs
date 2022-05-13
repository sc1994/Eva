using Eva.SingleSignOn.Options;
using Eva.SingleSignOn.ServiceInterfaces;
using Eva.SingleSignOn.ServiceInterfaces.JwtServices;
using Eva.SingleSignOn.Services.JwtServices;
using Eva.ToolKit.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.AddCustomSerilog();
builder.AddCustomAgileConfig();
builder.AddCustomAutoMapper();
builder.AddCustomHealthChecks();
builder.AddCustomFreeSql();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<JwtSetting>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ThrowFriendlyException>();

app.MapControllers();
app.UseCustomHealthChecks();


app.Run();