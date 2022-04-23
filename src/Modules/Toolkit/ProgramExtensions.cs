// Only use in this file to avoid conflicts with Microsoft.Extensions.Logging

using System.Reflection;
using AgileConfig.Client;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Eva.ToolKit;

public static class ProgramExtensions
{
    public static void AddCustomSerilog(this WebApplicationBuilder builder)
    {
        var seqServerUrl = builder.Configuration["SeqServerUrl"];
        var appName = AppDomain.CurrentDomain.FriendlyName;

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .WriteTo.Console()
            .WriteTo.Seq(seqServerUrl)
            .Enrich.WithProperty("ApplicationName", appName)
            .CreateLogger();

        builder.Host.UseSerilog();
    }

    public static void AddCustomAgileConfig(this WebApplicationBuilder builder)
    {
        void Options(ConfigClientOptions options)
        {
            options.AppId = builder.Configuration["AgileConfig:AppId"];
            options.Secret = builder.Configuration["AgileConfig:Secret"];
            options.Nodes = builder.Configuration["AgileConfig:Nodes"];
        }

        builder.Host.UseAgileConfig(Options);
    }

    public static void AddCustomAutoMapper(this WebApplicationBuilder builder)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        builder.Services.AddSingleton(provider =>
        {
            return new MapperConfiguration(cfg =>
            {
                assemblies
                    .SelectMany(x => x.GetTypes())
                    .ToList()
                    .ForEach(type =>
                    {
                        var mapperCaseList = type.GetCustomAttributes<MapToAttribute>().ToList();
                        if (mapperCaseList.Any() != true) return;

                        foreach (var mapperCase in mapperCaseList)
                        {
                            cfg.CreateProfile(
                                $"{type.FullName}_mutually_{mapperCase.MapToType.FullName}",
                                profileConfig => { profileConfig.CreateMap(type, mapperCase.MapToType).DisableCtorValidation().ReverseMap(); });
                        }
                    });
                cfg.AddMaps(assemblies);
            }).CreateMapper();
        });
    }
}