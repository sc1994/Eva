using Eva.HttpAggregator.Services.ProxyServices.Dto;

namespace Eva.HttpAggregator.ServiceInterfaces;

public interface IProxyService
{
    Task<bool> TryProxyAsync(HttpContext ctx);

    Task<string> TryProxySwaggerJsonAsync(HttpContext ctx);

    ProxySettingDto? GetConfig();
}