using Eva.ToolKit;

var builder = WebApplication.CreateBuilder(args);

builder.AddCustomSerilog();
builder.AddCustomAgileConfig();
builder.AddCustomAutoMapper();
builder.AddCustomHealthChecks();
builder.AddCustomFreeSql();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();