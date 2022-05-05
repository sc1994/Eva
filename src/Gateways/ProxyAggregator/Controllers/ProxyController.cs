using System.Net;
using Eva.ProxyAggregator.Options;

namespace Eva.ProxyAggregator.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class ProxyController : ControllerBase
{
    private readonly DaprClient _daprClient;
    private readonly ProxySetting _proxySetting;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProxyController(DaprClient daprClient, ProxySetting proxySetting, IHttpContextAccessor httpContextAccessor)
    {
        _daprClient = daprClient;
        _proxySetting = proxySetting;
        _httpContextAccessor = httpContextAccessor;
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

        var httpMethod = _httpContextAccessor.HttpContext?.Request.Method.ToLower();

        if (string.IsNullOrWhiteSpace(httpMethod))
        {
            throw new NullReferenceException(nameof(httpMethod));
        }

        if (service.Methods?.Contains(httpMethod) != true)
        {
            throw new NotSupportedException($"not supported {httpMethod}");
        }

        var method = httpMethod switch
        {
            "get" => HttpMethod.Get,
            "post" => HttpMethod.Post,
            "put" => HttpMethod.Put,
            "delete" => HttpMethod.Delete,
            "options" => HttpMethod.Options,
            "head" => HttpMethod.Head,
            _ => throw new NotSupportedException($"not supported {httpMethod}")
        };
        using var request = _daprClient.CreateInvokeMethodRequest(method, appid, proxyMethod);
        var response = await _daprClient.InvokeMethodWithResponseAsync(request);

        return new ContentResult
        {
            ContentType = response.Content.Headers.ContentType?.MediaType ?? "text/plain",
            Content = await response.Content.ReadAsStringAsync(),
            StatusCode = (int) response.StatusCode
        };
    }

    // [HttpGet]
    // [Route("/proxy/{appid}/{methodName}")]
    // [Route("/proxy/{appid}/{methodName}/{param1}")]
    // [Route("/proxy/{appid}/{methodName}/{param1}/{param2}")]
    // [Route("/proxy/{appid}/{methodName}/{param1}/{param2}/{param3}")]
    // [Route("/proxy/{appid}/{methodName}/{param1}/{param2}/{param3}/{param4}")]
    // [Route("/proxy/{appid}/{methodName}/{param1}/{param2}/{param3}/{param4}/{param5}")]
    // [Route("/proxy/{appid}/{methodName}/{param1}/{param2}/{param3}/{param4}/{param5}/{param6}")]
    // public async Task<ContentResult> HandleGet(string appid, string methodName)
    // {
    //     methodName = GetFullMethodName(methodName);
    //
    //     using var request = _daprClient.CreateInvokeMethodRequest(HttpMethod.Get, appid, methodName);
    //     var response = await _daprClient.InvokeMethodWithResponseAsync(request);
    //
    //     return new ContentResult
    //     {
    //         ContentType = response.Content.Headers.ContentType?.MediaType ?? "text/plain",
    //         Content = await response.Content.ReadAsStringAsync(),
    //         StatusCode = (int) response.StatusCode
    //     };
    // }
    //
    // [HttpPost]
    // [Route("/proxy/{appid}/{methodName}")]
    // [Route("/proxy/{appid}/{methodName}/{param1}")]
    // [Route("/proxy/{appid}/{methodName}/{param1}/{param2}")]
    // [Route("/proxy/{appid}/{methodName}/{param1}/{param2}/{param3}")]
    // [Route("/proxy/{appid}/{methodName}/{param1}/{param2}/{param3}/{param4}")]
    // [Route("/proxy/{appid}/{methodName}/{param1}/{param2}/{param3}/{param4}/{param5}")]
    // [Route("/proxy/{appid}/{methodName}/{param1}/{param2}/{param3}/{param4}/{param5}/{param6}")]
    // public async Task<ContentResult> HandlePost(string appid, string methodName, [FromBody] object body)
    // {
    //     methodName = GetFullMethodName(methodName);
    //
    //     using var request = _daprClient.CreateInvokeMethodRequest(HttpMethod.Post, appid, methodName, body);
    //     var response = await _daprClient.InvokeMethodWithResponseAsync(request);
    //
    //     return new ContentResult
    //     {
    //         ContentType = response.Content.Headers.ContentType?.MediaType ?? "text/plain",
    //         Content = await response.Content.ReadAsStringAsync(),
    //         StatusCode = (int) response.StatusCode
    //     };
    // }
    //
    // [HttpPut]
    // [Route("/proxy/{appid}/{methodName}")]
    // [Route("/proxy/{appid}/{methodName}/{param1}")]
    // [Route("/proxy/{appid}/{methodName}/{param1}/{param2}")]
    // [Route("/proxy/{appid}/{methodName}/{param1}/{param2}/{param3}")]
    // [Route("/proxy/{appid}/{methodName}/{param1}/{param2}/{param3}/{param4}")]
    // [Route("/proxy/{appid}/{methodName}/{param1}/{param2}/{param3}/{param4}/{param5}")]
    // [Route("/proxy/{appid}/{methodName}/{param1}/{param2}/{param3}/{param4}/{param5}/{param6}")]
    // public async Task<ContentResult> HandlePut(string appid, string methodName, [FromBody] object body)
    // {
    //     methodName = GetFullMethodName(methodName);
    //
    //     using var request = _daprClient.CreateInvokeMethodRequest(HttpMethod.Put, appid, methodName, body);
    //     var response = await _daprClient.InvokeMethodWithResponseAsync(request);
    //
    //     return new ContentResult
    //     {
    //         ContentType = response.Content.Headers.ContentType?.MediaType ?? "text/plain",
    //         Content = await response.Content.ReadAsStringAsync(),
    //         StatusCode = (int) response.StatusCode
    //     };
    // }
    //
    // [HttpDelete]
    // [Route("/proxy/{appid}/{methodName}")]
    // [Route("/proxy/{appid}/{methodName}/{param1}")]
    // [Route("/proxy/{appid}/{methodName}/{param1}/{param2}")]
    // [Route("/proxy/{appid}/{methodName}/{param1}/{param2}/{param3}")]
    // [Route("/proxy/{appid}/{methodName}/{param1}/{param2}/{param3}/{param4}")]
    // [Route("/proxy/{appid}/{methodName}/{param1}/{param2}/{param3}/{param4}/{param5}")]
    // [Route("/proxy/{appid}/{methodName}/{param1}/{param2}/{param3}/{param4}/{param5}/{param6}")]
    // public async Task<ContentResult> HandleDelete(string appid, string methodName)
    // {
    //     methodName = GetFullMethodName(methodName);
    //
    //     using var request = _daprClient.CreateInvokeMethodRequest(HttpMethod.Delete, appid, methodName);
    //     var response = await _daprClient.InvokeMethodWithResponseAsync(request);
    //
    //     return new ContentResult
    //     {
    //         ContentType = response.Content.Headers.ContentType?.MediaType ?? "text/plain",
    //         Content = await response.Content.ReadAsStringAsync(),
    //         StatusCode = (int) response.StatusCode
    //     };
    // }
    //
    // private string GetFullMethodName(string methodName)
    // {
    //     var paramCount = 1;
    //     while (true)
    //     {
    //         var param = _httpContextAccessor.HttpContext?.Request.RouteValues[$"param{paramCount++}"];
    //         if (param == null)
    //         {
    //             break;
    //         }
    //
    //         methodName += $"/{param}";
    //     }
    //
    //     return methodName;
    // }
}