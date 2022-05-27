namespace Eva.Aggregator.Options;

public record ProxyHeader
{
    [JsonProperty("request")] public IEnumerable<string>? Request { get; set; }

    [JsonProperty("response")] public IEnumerable<string>? Response { get; set; }
}