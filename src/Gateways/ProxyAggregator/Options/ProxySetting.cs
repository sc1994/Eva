namespace Eva.ProxyAggregator.Options;

public class ProxySetting
{
    [JsonProperty("services")] public IEnumerable<ProxyService>? Services { get; set; }
}