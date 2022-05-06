var builder = WebApplication.CreateBuilder(args);

builder.AddCustomAgileConfig();
builder.AddCustomSerilog();
builder.AddCustomAutoMapper();
builder.AddCustomHealthChecks();
builder.AddCustomFreeSql();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDaprClient();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.MapSubscribeHandler();

app.UseCustomHealthChecks();

app.Run();