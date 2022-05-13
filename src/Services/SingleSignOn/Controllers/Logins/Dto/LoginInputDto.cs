namespace Eva.SingleSignOn.Controllers.Logins.Dto;

public record LoginInputDto([Required] string UserName, [Required] string Password);