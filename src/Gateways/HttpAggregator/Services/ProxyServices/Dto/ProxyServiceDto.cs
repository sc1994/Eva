namespace Eva.HttpAggregator.Services.ProxyServices.Dto;

public class ProxyServiceDto
{
    [JsonProperty("appid")]
    public string? Appid { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("methods")]
    public List<string>? Methods { get; set; }

    [JsonProperty("headers")]
    public ProxyHeadersDto? Headers { get; set; }

    [JsonProperty("swagger")]
    public string? Swagger { get; set; }
}