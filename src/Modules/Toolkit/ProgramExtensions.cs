using System.Reflection;
using AgileConfig.Client;
using AutoMapper;
using Eva.ToolKit.Attributes;
using FluentValidation.AspNetCore;
using FreeSql;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Eva.ToolKit;

public static class ProgramExtensions
{
    public static void AddCustomSerilog(this WebApplicationBuilder builder)
    {
        var logConfig = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .WriteTo.Console();

        var seqServerUrl = builder.Configuration["SeqServerUrl"];
        if (!string.IsNullOrWhiteSpace(seqServerUrl))
        {
            var appName = AppDomain.CurrentDomain.FriendlyName;
            logConfig.WriteTo.Seq(seqServerUrl)
                .Enrich.WithProperty("ApplicationName", appName);
        }

        logConfig.Filter.With<LogConfigFilter>();

        Log.Logger = logConfig.CreateLogger();

        builder.Host.UseSerilog();
    }

    private class LogConfigFilter : ILogEventFilter
    {
        public bool IsEnabled(LogEvent logEvent)
        {
            if (logEvent.Properties.ContainsKey("RequestPath"))
            {
                var path = (logEvent.Properties["RequestPath"] as ScalarValue)?.Value.ToString();
                if (path is "/hc" or "/liveness")
                {
                    return false;
                }
            }

            return true;
        }
    }

    public static void AddCustomAgileConfig(this WebApplicationBuilder builder)
    {
        void Options(ConfigClientOptions options)
        {
            options.AppId = builder.Configuration["AgileConfig_AppId"];
            options.Secret = builder.Configuration["AgileConfig_Secret"];
            options.Nodes = builder.Configuration["AgileConfig_Nodes"];
        }

        builder.Host.UseAgileConfig(Options);
    }

    public static void AddCustomAutoMapper(this WebApplicationBuilder builder)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        builder.Services.AddSingleton(_ =>
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
                            cfg.CreateProfile(
                                $"{type.FullName}_mutually_{mapperCase.MapToType.FullName}",
                                profileConfig => { profileConfig.CreateMap(type, mapperCase.MapToType).DisableCtorValidation().ReverseMap(); });
                    });
                cfg.AddMaps(assemblies);
            }).CreateMapper();
        });
    }

    public static void AddCustomHealthChecks(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy())
            .AddCheck<DaprHealthCheck>("dapr");
    }

    public static void UseCustomHealthChecks(this IEndpointRouteBuilder app)
    {
        app.MapHealthChecks("/hc", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        app.MapHealthChecks("/liveness", new HealthCheckOptions
        {
            Predicate = r => r.Name.Contains("self")
        });
    }

    public static void AddCustomFreeSql(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IFreeSql>(_ =>
        {
            var freeSql = new FreeSqlBuilder()
                .UseConnectionString(DataType.MySql, builder.Configuration["DatabaseConnection"])
                .UseAutoSyncStructure(true)
                .Build();
            return freeSql;
        });
    }

    public static void AddCustomController(this WebApplicationBuilder builder, Assembly thisAssembly)
    {
        builder.Services.AddControllers(options => { options.Filters.Add<ValidateModelStateAttribute>(); })
            .AddFluentValidation(configuration => { configuration.RegisterValidatorsFromAssembly(thisAssembly); })
            .ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
    }

    public static void UseStaticDIUtility(this IEndpointRouteBuilder app)
    {
        DIUtility.SetGlobalServiceProvider(app.ServiceProvider);
    }

    public static void AddCustomSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. <br /><br /> 
                              Enter 'Bearer' [space] and then your token in the text input below.
                              <br /><br />Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            }
        );
    }
}