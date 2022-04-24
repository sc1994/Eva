using Eva.HttpAggregator.ServiceInterfaces;

namespace Eva.HttpAggregator.Services.DemandsServices;

public class DemandsService : IDemandsService
{
    private readonly HttpClient _httpClient;

    public DemandsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> CreateAsync(object request)
    {
        var ctn = JsonContent.Create(request);
        var response = await _httpClient.PostAsync("/api/Demands", ctn);

        return await response.Content.ReadAsStringAsync();
    }
}