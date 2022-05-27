var builder = WebApplication.CreateBuilder(args);

builder.AddCustomController(typeof(Program).Assembly);
builder.AddCustomAgileConfig();
builder.AddCustomSerilog();
builder.AddCustomAutoMapper();
builder.AddCustomHealthChecks();
builder.AddCustomFreeSql();

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