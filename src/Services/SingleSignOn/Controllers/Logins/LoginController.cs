using Eva.SingleSignOn.Controllers.Logins.Dto;

namespace Eva.SingleSignOn.Controllers.Logins;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    [HttpPost]
    public IActionResult Login(LoginDto input)
    {
        throw new NotImplementedException();
    }
    
    
}