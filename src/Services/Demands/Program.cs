using Eva.ToolKit;

var builder = WebApplication.CreateBuilder(args);

builder.AddCustomAgileConfig();

builder.AddCustomSerilog();
builder.AddCustomAutoMapper();

builder.Services.AddSingleton<IFreeSql>(_ =>
{
    var freeSql = new FreeSql.FreeSqlBuilder()
        .UseConnectionString(FreeSql.DataType.MySql, builder.Configuration["DatabaseConnection"])
        .UseAutoSyncStructure(true) //自动迁移实体的结构到数据库
        .Build(); //请务必定义成 Singleton 单例模式

    return freeSql;
});

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