namespace Eva.SingleSignOn.Controllers.Logins.Dto;

public record TokenOutputDto(string Token, int Expiration);