using Eva.ToolKit;

var builder = WebApplication.CreateBuilder(args);

builder.AddCustomSerilog();
builder.AddCustomAutoMapper();
builder.AddCustomAgileConfig();

builder.Services.AddSingleton<IFreeSql>(provider =>
{
    var freeSql = new FreeSql.FreeSqlBuilder()
        .UseConnectionString(FreeSql.DataType.Sqlite, @"Data Source=|DataDirectory|\demands.db;Pooling=true;Max Pool Size=10")
        .UseAutoSyncStructure(true) //自动迁移实体的结构到数据库
        .Build(); //请务必定义成 Singleton 单例模式

    return freeSql;
});



var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();