using System.Net;
using System.Text;
using Eva.ProxyAggregator.Options;

namespace Eva.ProxyAggregator.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class ProxyController : ControllerBase
{
    private readonly DaprClient _daprClient;
    private readonly ProxySetting _proxySetting;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<ProxyController> _logger;

    public ProxyController(DaprClient daprClient, ProxySetting proxySetting, IHttpContextAccessor httpContextAccessor, ILogger<ProxyController> logger)
    {
        _daprClient = daprClient;
        _proxySetting = proxySetting;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    [HttpGet]
    [Route("/swagger/{appid}/v1/swagger.json")]
    public async Task<ContentResult> GetSwaggerJson(string appid)
    {
        var methodName = _proxySetting.Services?.FirstOrDefault(x => x.Appid == appid)?.Swagger;
        if (string.IsNullOrEmpty(methodName))
        {
            throw new NullReferenceException(nameof(methodName));
        }

        using var request = _daprClient.CreateInvokeMethodRequest(HttpMethod.Get, appid, methodName);
        var response = await _daprClient.InvokeMethodWithResponseAsync(request);
        var data = await response.Content.ReadAsStringAsync();

        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new ServiceRequestException($"Failed to get {methodName} from {appid}", response.StatusCode, data);
        }

        var swagger = JsonConvert.DeserializeObject<JObject>(data);
        var tmp = new List<JProperty>();
        foreach (var jToken in swagger?["paths"] ?? throw new NullReferenceException("swagger?[\"paths\"]"))
        {
            var item = (JProperty) jToken;
            tmp.Add(item);
        }

        foreach (var item in tmp)
        {
            item.Replace(new JProperty($"/proxy/{appid}{item.Name}", item.Value));
        }

        return new ContentResult
        {
            ContentType = "application/json",
            Content = swagger.ToString(),
            StatusCode = (int) HttpStatusCode.OK
        };
    }

    [HttpGet("/proxy/{appid}/{**proxyMethod}")]
    [HttpPost("/proxy/{appid}/{**proxyMethod}")]
    [HttpPut("/proxy/{appid}/{**proxyMethod}")]
    [HttpDelete("/proxy/{appid}/{**proxyMethod}")]
    [HttpOptions("/proxy/{appid}/{**proxyMethod}")]
    [HttpHead("/proxy/{appid}/{**proxyMethod}")]
    public async Task<ContentResult> Handle(string appid, string proxyMethod)
    {
        var service = _proxySetting.Services?.FirstOrDefault(x => x.Appid == appid);
        if (service == null)
        {
            throw new NullReferenceException(nameof(service));
        }

        var request = _httpContextAccessor.HttpContext?.Request;

        if (request == null)
        {
            throw new NullReferenceException(nameof(request));
        }

        var httpMethod = request.Method.ToLower();

        if (string.IsNullOrWhiteSpace(httpMethod))
        {
            throw new NullReferenceException(nameof(httpMethod));
        }

        if (service.Methods?.Contains(httpMethod) != true)
        {
            throw new NotSupportedException($"not supported {httpMethod}");
        }

        using var proxyRequest = _daprClient.CreateInvokeMethodRequest(new HttpMethod(httpMethod), appid, proxyMethod);
        if (!HttpMethods.IsGet(httpMethod) &&
            !HttpMethods.IsHead(httpMethod) &&
            !HttpMethods.IsDelete(httpMethod) &&
            !HttpMethods.IsTrace(httpMethod))
        {
            var streamContent = new StreamContent(request.Body);
            proxyRequest.Content = streamContent;
        }

        if (!string.IsNullOrWhiteSpace(request.QueryString.Value))
        {
            proxyRequest.RequestUri = new Uri(proxyRequest.RequestUri ?? throw new NullReferenceException(nameof(proxyRequest.RequestUri)), request.QueryString.Value);
        }

        var response = await _daprClient.InvokeMethodWithResponseAsync(proxyRequest);

        return new ContentResult
        {
            ContentType = response.Content.Headers.ContentType?.MediaType ?? "text/plain",
            Content = await response.Content.ReadAsStringAsync(),
            StatusCode = (int) response.StatusCode
        };
    }
}