namespace Eva.ProxyAggregator.Options;

public record ProxyService
{
    [JsonProperty("appid")] public string? Appid { get; set; }

    [JsonProperty("description")] public string? Description { get; set; }

    [JsonProperty("methods")] public List<string>? Methods { get; set; }

    [JsonProperty("headers")] public ProxyHeader? Headers { get; set; }

    [JsonProperty("swagger")] public string? Swagger { get; set; }
}