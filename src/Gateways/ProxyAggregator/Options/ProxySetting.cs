namespace Eva.ProxyAggregator.Options;

public record ProxySetting
{
    [JsonProperty("services")] public IEnumerable<ProxyService>? Services { get; set; }
}