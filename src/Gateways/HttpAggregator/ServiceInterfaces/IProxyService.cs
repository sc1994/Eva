using Eva.HttpAggregator.Services.ProxyServices.Dto;

namespace Eva.HttpAggregator.ServiceInterfaces;

public interface IProxyService
{
    ProxySettingDto? GetConfig();
}