namespace Eva.ProxyAggregator.Options;

public class ProxyHeader
{
    [JsonProperty("request")]
    public IEnumerable<string>? Request { get; set; }

    [JsonProperty("response")]
    public IEnumerable<string>? Response { get; set; }
}