using Eva.HttpAggregator.ServiceInterfaces;
using Eva.HttpAggregator.Services.ProxyServices.Dto;

namespace Eva.HttpAggregator.Services.ProxyServices;

public class ProxyService : IProxyService
{
    private readonly IConfiguration _configuration;
    public ProxyService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ProxySettingDto? GetConfig()
    {
        var configString = _configuration["ProxySettings"];
        if (configString == null)
            return null;

        var config = JsonConvert.DeserializeObject<ProxySettingDto>(configString); // TODO 使用 bind + changed 事件 获取最新配置

        return config ?? throw new ArgumentNullException(nameof(config));
    }
}