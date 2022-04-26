using Eva.HttpAggregator.ServiceInterfaces;
using Eva.HttpAggregator.Services.ProxyServices.Dto;

namespace Eva.HttpAggregator.Services.ProxyServices;

public class ProxyService : IProxyService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ProxyService> _logger;
    private readonly DaprClient _daprClient;

    public ProxyService(IConfiguration configuration, ILogger<ProxyService> logger, DaprClient daprClient)
    {
        _configuration = configuration;
        _logger = logger;
        _daprClient = daprClient;
    }

    public async Task<bool> TryProxyAsync(HttpContext ctx)
    {
        if (!GetServiceConfig(ctx, out var serviceConfig) || serviceConfig == null)
            return false;

        if (ctx.Request.Path.Value?.EndsWith("swagger.json") == true)
            return false;

        var response = await SendUseDaprClientAsync(ctx, serviceConfig);
        var responseData = await response.Content.ReadAsStringAsync();
        await ctx.Response.WriteAsync(responseData);

        return true;
    }

    public async Task<string> TryProxySwaggerJsonAsync(HttpContext ctx)
    {
        var method = ctx.Request.Method.ToLower() switch
        {
            "get" => HttpMethod.Get,
            "post" => HttpMethod.Post,
            "put" => HttpMethod.Put,
            "delete" => HttpMethod.Delete,
            _ => throw new NotSupportedException("HttpMethod:" + ctx.Request.Method)
        };

        if (!GetServiceConfig(ctx, out var serviceConfig) || serviceConfig == null)
            throw new Exception("ServiceConfig not found");

        var response = await SendUseHttpClientAsync(ctx, serviceConfig, method);

        return await response.Content.ReadAsStringAsync();
    }

    private bool GetServiceConfig(HttpContext ctx, out ProxyServiceDto? serviceConfig)
    {
        serviceConfig = null;
        var config = GetConfig();

        if (config?.Services?.Any() != true)
            return false;

        var paths = ctx.Request.Path.Value?.Split("/", StringSplitOptions.RemoveEmptyEntries);
        if (paths?.Any() != true)
            return false;

        var appId = paths.First();
        if (string.IsNullOrWhiteSpace(appId))
            throw new NullReferenceException(nameof(appId));

        serviceConfig = config.Services.FirstOrDefault(x => x.Appid == appId);
        if (serviceConfig == null || string.IsNullOrWhiteSpace(serviceConfig.Appid))
            return false;

        if (serviceConfig.Methods?.Select(x => x.ToLower()).Contains(ctx.Request.Method.ToLower()) != true)
            return false;

        return true;
    }

    private async Task<HttpResponseMessage> SendUseHttpClientAsync(HttpContext ctx, ProxyServiceDto serviceConfig, HttpMethod method)
    {
        using var request = new HttpRequestMessage(method, GetSendPath(ctx, serviceConfig));
        SetRequestHeaders(ctx, serviceConfig, request);

        var response = await DaprClient.CreateInvokeHttpClient(serviceConfig.Appid).SendAsync(request);
        SetResponseBase(ctx, serviceConfig, response);

        return response;
    }

    private async Task<HttpResponseMessage> SendUseDaprClientAsync(HttpContext ctx, ProxyServiceDto serviceConfig)
    {
        using var request = _daprClient.CreateInvokeMethodRequest(serviceConfig.Appid, GetSendPath(ctx, serviceConfig));
        SetRequestHeaders(ctx, serviceConfig, request);

        var response = await _daprClient.InvokeMethodWithResponseAsync(request);
        SetResponseBase(ctx, serviceConfig, response);

        return response;
    }

    private string GetSendPath(HttpContext ctx, ProxyServiceDto serviceConfig)
    {
        var paths = ctx.Request.Path.Value?
            .Split("/", StringSplitOptions.RemoveEmptyEntries)
            .Skip(1) ?? throw new NullReferenceException("ctx.Request.Path");

        return $"/{string.Join("/", paths)}";
    }

    public ProxySettingDto? GetConfig()
    {
        var configString = _configuration["ProxySettings"];
        if (configString == null)
            return null;

        var config = JsonConvert.DeserializeObject<ProxySettingDto>(configString); // TODO 使用 bind + changed 事件 获取最新配置

        return config ?? throw new ArgumentNullException(nameof(config));
    }

    private void SetRequestHeaders(HttpContext ctx, ProxyServiceDto serviceConfig, HttpRequestMessage request)
    {
        if (serviceConfig.Headers?.Request?.Any() != true) return;

        foreach (var item in serviceConfig.Headers.Request)
        {
            if (!ctx.Request.Headers.Keys.Contains(item))
                continue;

            request.Headers.Add(item, ctx.Request.Headers[item].ToString());
        }
    }

    private void SetResponseBase(HttpContext ctx, ProxyServiceDto serviceConfig, HttpResponseMessage response)
    {
        ctx.Response.StatusCode = response.StatusCode.GetHashCode();

        // if (!serviceConfig.Headers.Request.Any()) return;
        //
        // foreach (var item in serviceConfig.Headers.Response)
        // {
        //     if (ctx.Response.Headers.Keys.Contains(item))
        //     {
        //         
        //     }
        //     else
        //     {
        //     }
        // }

        // TODO 设置响应头
    }
}