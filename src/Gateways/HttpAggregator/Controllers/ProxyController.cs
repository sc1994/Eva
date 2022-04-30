using System.Net;
using Dapr;
using Eva.HttpAggregator.ServiceInterfaces;

namespace Eva.HttpAggregator.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class ProxyController : ControllerBase
{
    private readonly IProxyService _proxyService;
    private readonly DaprClient _daprClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProxyController(DaprClient daprClient, IProxyService proxyService, IHttpContextAccessor httpContextAccessor)
    {
        _daprClient = daprClient;
        _proxyService = proxyService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    [Route("/{appid}/swagger/v1/swagger.json")]
    public async Task<ContentResult> GetSwaggerJson(string appid)
    {
        var config = _proxyService.GetConfig();
        var methodName = config?.Services?.FirstOrDefault(x => x.Appid == appid)?.Swagger;
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

    [HttpGet]
    [Route("/proxy/{appid}/{methodName}")]
    [Route("/proxy/{appid}/{methodName}/{param1}")]
    [Route("/proxy/{appid}/{methodName}/{param1}/{param2}")]
    [Route("/proxy/{appid}/{methodName}/{param1}/{param2}/{param3}")]
    [Route("/proxy/{appid}/{methodName}/{param1}/{param2}/{param3}/{param4}")]
    [Route("/proxy/{appid}/{methodName}/{param1}/{param2}/{param3}/{param4}/{param5}")]
    [Route("/proxy/{appid}/{methodName}/{param1}/{param2}/{param3}/{param4}/{param5}/{param6}")]
    public async Task<ContentResult> HandleGet(string appid, string methodName)
    {
        methodName = GetFullMethodName(methodName);

        using var request = _daprClient.CreateInvokeMethodRequest(HttpMethod.Get, appid, methodName);
        var response = await _daprClient.InvokeMethodWithResponseAsync(request);

        return new ContentResult
        {
            ContentType = response.Content.Headers.ContentType?.MediaType ?? "text/plain",
            Content = await response.Content.ReadAsStringAsync(),
            StatusCode = (int) response.StatusCode
        };
    }

    [HttpPost]
    [Route("/proxy/{appid}/{methodName}")]
    [Route("/proxy/{appid}/{methodName}/{param1}")]
    [Route("/proxy/{appid}/{methodName}/{param1}/{param2}")]
    [Route("/proxy/{appid}/{methodName}/{param1}/{param2}/{param3}")]
    [Route("/proxy/{appid}/{methodName}/{param1}/{param2}/{param3}/{param4}")]
    [Route("/proxy/{appid}/{methodName}/{param1}/{param2}/{param3}/{param4}/{param5}")]
    [Route("/proxy/{appid}/{methodName}/{param1}/{param2}/{param3}/{param4}/{param5}/{param6}")]
    public async Task<ContentResult> HandlePost(string appid, string methodName, [FromBody] object body)
    {
        methodName = GetFullMethodName(methodName);

        using var request = _daprClient.CreateInvokeMethodRequest(HttpMethod.Post, appid, methodName, body);
        var response = await _daprClient.InvokeMethodWithResponseAsync(request);

        return new ContentResult
        {
            ContentType = response.Content.Headers.ContentType?.MediaType ?? "text/plain",
            Content = await response.Content.ReadAsStringAsync(),
            StatusCode = (int) response.StatusCode
        };
    }

    [HttpPut]
    [Route("/proxy/{appid}/{methodName}")]
    [Route("/proxy/{appid}/{methodName}/{param1}")]
    [Route("/proxy/{appid}/{methodName}/{param1}/{param2}")]
    [Route("/proxy/{appid}/{methodName}/{param1}/{param2}/{param3}")]
    [Route("/proxy/{appid}/{methodName}/{param1}/{param2}/{param3}/{param4}")]
    [Route("/proxy/{appid}/{methodName}/{param1}/{param2}/{param3}/{param4}/{param5}")]
    [Route("/proxy/{appid}/{methodName}/{param1}/{param2}/{param3}/{param4}/{param5}/{param6}")]
    public async Task<ContentResult> HandlePut(string appid, string methodName, [FromBody] object body)
    {
        methodName = GetFullMethodName(methodName);

        using var request = _daprClient.CreateInvokeMethodRequest(HttpMethod.Put, appid, methodName, body);
        var response = await _daprClient.InvokeMethodWithResponseAsync(request);

        return new ContentResult
        {
            ContentType = response.Content.Headers.ContentType?.MediaType ?? "text/plain",
            Content = await response.Content.ReadAsStringAsync(),
            StatusCode = (int) response.StatusCode
        };
    }

    [HttpDelete]
    [Route("/proxy/{appid}/{methodName}")]
    [Route("/proxy/{appid}/{methodName}/{param1}")]
    [Route("/proxy/{appid}/{methodName}/{param1}/{param2}")]
    [Route("/proxy/{appid}/{methodName}/{param1}/{param2}/{param3}")]
    [Route("/proxy/{appid}/{methodName}/{param1}/{param2}/{param3}/{param4}")]
    [Route("/proxy/{appid}/{methodName}/{param1}/{param2}/{param3}/{param4}/{param5}")]
    [Route("/proxy/{appid}/{methodName}/{param1}/{param2}/{param3}/{param4}/{param5}/{param6}")]
    public async Task<ContentResult> HandleDelete(string appid, string methodName)
    {
        methodName = GetFullMethodName(methodName);

        using var request = _daprClient.CreateInvokeMethodRequest(HttpMethod.Delete, appid, methodName);
        var response = await _daprClient.InvokeMethodWithResponseAsync(request);

        return new ContentResult
        {
            ContentType = response.Content.Headers.ContentType?.MediaType ?? "text/plain",
            Content = await response.Content.ReadAsStringAsync(),
            StatusCode = (int) response.StatusCode
        };
    }

    private string GetFullMethodName(string methodName)
    {
        var paramCount = 1;
        while (true)
        {
            var param = _httpContextAccessor.HttpContext?.Request.RouteValues[$"param{paramCount++}"];
            if (param == null)
            {
                break;
            }

            methodName += $"/{param}";
        }

        return methodName;
    }
}