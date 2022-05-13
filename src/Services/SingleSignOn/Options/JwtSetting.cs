namespace Eva.SingleSignOn.Options;

public record JwtSetting
{
    public string SecretKey { get; set; } = string.Empty;
    public int Expiration { get; set; }
}