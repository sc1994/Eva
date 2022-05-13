using System.Net;
using Eva.ToolKit.Exceptions;

namespace Eva.ProxyAggregator.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class ProxyController : ControllerBase
{
    private readonly DaprClient _daprClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ProxySetting _proxySetting;

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
        var swaggerUrl = _proxySetting.Services?.FirstOrDefault(x => x.Appid == appid)?.Swagger;
        if (string.IsNullOrEmpty(swaggerUrl)) throw new NullReferenceException(nameof(swaggerUrl));

        using var request = _daprClient.CreateInvokeMethodRequest(HttpMethod.Get, appid, swaggerUrl);

        HttpResponseMessage response;
        try
        {
            response = await _daprClient.InvokeMethodWithResponseAsync(request);
        }
        catch (Exception e)
        {
            throw new BusinessException($"Failed to invoke method [{swaggerUrl}]", innerException: e);
        }

        var data = await response.Content.ReadAsStringAsync();
        if (response.StatusCode != HttpStatusCode.OK)
            throw new BusinessException($"Failed to get {swaggerUrl} from {appid}", $"{response.StatusCode}_000", data);

        var swagger = data.ConvertJsonToObject<JObject>() ?? throw new NullReferenceException("swagger");

        ReSetPath(swagger, appid);

        return new ContentResult
        {
            ContentType = AppConst.ContentJson,
            Content = swagger.ToString(),
            StatusCode = (int) HttpStatusCode.OK
        };
    }

    private void ReSetPath(JObject swagger, string appid)
    {
        var tmp = new List<JProperty>();
        foreach (var jToken in swagger["paths"] ?? throw new NullReferenceException("swagger?[\"paths\"]"))
        {
            var item = (JProperty) jToken;
            tmp.Add(item);
        }

        foreach (var item in tmp) item.Replace(new JProperty($"/proxy/{appid}{item.Name}", item.Value));
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
        if (service == null) throw new NullReferenceException(nameof(service));

        var request = _httpContextAccessor.HttpContext?.Request;

        if (request == null) throw new NullReferenceException(nameof(request));

        var httpMethod = request.Method.ToLower();

        if (string.IsNullOrWhiteSpace(httpMethod)) throw new NullReferenceException(nameof(httpMethod));

        if (service.Methods?.Contains(httpMethod) != true) throw new NotSupportedException($"not supported {httpMethod}");

        using var proxyRequest = _daprClient.CreateInvokeMethodRequest(new HttpMethod(httpMethod), appid, proxyMethod);
        if (!HttpMethods.IsGet(httpMethod) &&
            !HttpMethods.IsHead(httpMethod) &&
            !HttpMethods.IsDelete(httpMethod) &&
            !HttpMethods.IsTrace(httpMethod))
        {
            var streamContent = new StreamContent(request.Body);
            proxyRequest.Content = streamContent;
        }

        if (!string.IsNullOrWhiteSpace(request.QueryString.Value)) proxyRequest.RequestUri = new Uri(proxyRequest.RequestUri ?? throw new NullReferenceException(nameof(proxyRequest.RequestUri)), request.QueryString.Value);

        HttpResponseMessage response;
        try
        {
            response = await _daprClient.InvokeMethodWithResponseAsync(proxyRequest);
        }
        catch (Exception e)
        {
            throw new BusinessException($"Failed to invoke method [{proxyMethod}]", innerException: e);
        }

        var data = await response.Content.ReadAsStringAsync();
        if (response.StatusCode != HttpStatusCode.OK)
            throw new BusinessException($"Failed to get {proxyMethod} from {appid}", $"{response.StatusCode}_000", data);

        return new ContentResult
        {
            ContentType = response.Content.Headers.ContentType?.MediaType ?? AppConst.ContentText,
            Content = await response.Content.ReadAsStringAsync(),
            StatusCode = (int) HttpStatusCode.OK
        };
    }
}