using Eva.SingleSignOn.Options;
using Eva.SingleSignOn.ServiceInterfaces.JwtServices;
using Eva.SingleSignOn.ServiceInterfaces.RoleBindUserServices;
using Eva.SingleSignOn.ServiceInterfaces.RoleServices;
using Eva.SingleSignOn.ServiceInterfaces.UserServices;
using Eva.SingleSignOn.Services.JwtServices;
using Eva.SingleSignOn.Services.RoleBindUserServices;
using Eva.SingleSignOn.Services.RoleServices;
using Eva.SingleSignOn.Services.UserServices;
using Eva.ToolKit.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.AddCustomController(typeof(Program).Assembly);

builder.AddCustomAgileConfig();
builder.AddCustomSerilog();
builder.AddCustomAutoMapper();
builder.AddCustomHealthChecks();
builder.AddCustomFreeSql();
builder.AddCustomSwagger();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<ThrowFriendlyException>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<JwtSetting>(provider =>
{
    try
    {
        var json = builder.Configuration["JwtSetting"];

        return json.ConvertJsonToObject<JwtSetting>() ?? new JwtSetting();
    }
    catch (Exception e)
    {
        provider.GetRequiredService<ILogger<Program>>().LogError(e, "Failed to load jwt settings");
        return new JwtSetting();
    }
});
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleBindUserService, RoleBindUserService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ThrowFriendlyException>();

app.MapControllers();
app.UseCustomHealthChecks();
app.UseStaticDIUtility();


app.Run();