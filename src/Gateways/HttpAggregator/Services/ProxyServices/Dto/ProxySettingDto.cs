namespace Eva.HttpAggregator.Services.ProxyServices.Dto;

public class ProxySettingDto
{
    [JsonProperty("services")]
    public IEnumerable<ProxyServiceDto>? Services { get; set; }
}