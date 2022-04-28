using Eva.HttpAggregator.ServiceInterfaces;

namespace Eva.HttpAggregator.Controllers.DemandsItems;

[ApiController]
[Route("aggr/v1/[controller]")]
public class DemandsController : ControllerBase
{
    private readonly IDemandsService _demandsService;

    public DemandsController(IDemandsService demandsService)
    {
        _demandsService = demandsService;
    }
}