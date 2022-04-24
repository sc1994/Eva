using Eva.HttpAggregator.ServiceInterfaces;

namespace Eva.HttpAggregator.Services.DemandsServices;

public class DemandsService : IDemandsService
{
    private HttpClient _httpClient;

    public DemandsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    
}