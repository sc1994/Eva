namespace Eva.HttpAggregator.Services.ProxyServices.Dto;

public class ProxyHeadersDto
{
    [JsonProperty("request")]
    public IEnumerable<string>? Request { get; set; }

    [JsonProperty("response")]
    public IEnumerable<string>? Response { get; set; }
}