using System.Net;

namespace Eva.ProxyAggregator.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class ProxyController : ControllerBase
{
    private readonly DaprClient _daprClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ProxySetting _proxySetting;
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
        var swaggerUrl = _proxySetting.Services?.FirstOrDefault(x => x.Appid == appid)?.Swagger;
        if (string.IsNullOrEmpty(swaggerUrl)) throw new NullReferenceException(nameof(swaggerUrl));

        using var request = _daprClient.CreateInvokeMethodRequest(HttpMethod.Get, appid, swaggerUrl);

        HttpResponseMessage response;
        // try
        // {
        //     response = await _daprClient.InvokeMethodWithResponseAsync(request);
        // }
        // catch (Exception e)
        // {
        //     throw new ProxyException($"Failed to invoke method [{swaggerUrl}]", e);
        // }

        // var data = await response.Content.ReadAsStringAsync();
        // if (response.StatusCode != HttpStatusCode.OK) throw new ProxyException($"Failed to get {swaggerUrl} from {appid}", response.StatusCode, data);

        var data =
            "{\"openapi\":\"3.0.1\",\"info\":{\"title\":\"Demands\",\"version\":\"1.0\"},\"paths\":{\"/Demands/ModifyState\":{\"put\":{\"tags\":[\"Demands\"],\"parameters\":[{\"name\":\"id\",\"in\":\"query\",\"schema\":{\"type\":\"string\",\"format\":\"uuid\"}},{\"name\":\"state\",\"in\":\"query\",\"schema\":{\"$ref\":\"#/components/schemas/DemandState\"}}],\"responses\":{\"200\":{\"description\":\"Success\",\"content\":{\"text/plain\":{\"schema\":{\"type\":\"boolean\"}},\"application/json\":{\"schema\":{\"type\":\"boolean\"}},\"text/json\":{\"schema\":{\"type\":\"boolean\"}}}}}}},\"/Demands/now\":{\"get\":{\"tags\":[\"Demands\"],\"responses\":{\"200\":{\"description\":\"Success\",\"content\":{\"text/plain\":{\"schema\":{\"type\":\"string\"}},\"application/json\":{\"schema\":{\"type\":\"string\"}},\"text/json\":{\"schema\":{\"type\":\"string\"}}}}}}},\"/Demands/{id}\":{\"get\":{\"tags\":[\"Demands\"],\"parameters\":[{\"name\":\"id\",\"in\":\"path\",\"required\":true,\"schema\":{\"type\":\"string\",\"format\":\"uuid\"}}],\"responses\":{\"200\":{\"description\":\"Success\",\"content\":{\"text/plain\":{\"schema\":{\"$ref\":\"#/components/schemas/DemandsOutputDto\"}},\"application/json\":{\"schema\":{\"$ref\":\"#/components/schemas/DemandsOutputDto\"}},\"text/json\":{\"schema\":{\"$ref\":\"#/components/schemas/DemandsOutputDto\"}}}}}},\"delete\":{\"tags\":[\"Demands\"],\"parameters\":[{\"name\":\"id\",\"in\":\"path\",\"required\":true,\"schema\":{\"type\":\"string\",\"format\":\"uuid\"}}],\"responses\":{\"200\":{\"description\":\"Success\",\"content\":{\"text/plain\":{\"schema\":{\"type\":\"boolean\"}},\"application/json\":{\"schema\":{\"type\":\"boolean\"}},\"text/json\":{\"schema\":{\"type\":\"boolean\"}}}}}},\"put\":{\"tags\":[\"Demands\"],\"parameters\":[{\"name\":\"id\",\"in\":\"path\",\"required\":true,\"schema\":{\"type\":\"string\",\"format\":\"uuid\"}}],\"requestBody\":{\"content\":{\"application/json\":{\"schema\":{\"$ref\":\"#/components/schemas/DemandsModifiedDto\"}},\"text/json\":{\"schema\":{\"$ref\":\"#/components/schemas/DemandsModifiedDto\"}},\"application/*+json\":{\"schema\":{\"$ref\":\"#/components/schemas/DemandsModifiedDto\"}}},\"required\":true},\"responses\":{\"200\":{\"description\":\"Success\",\"content\":{\"text/plain\":{\"schema\":{\"$ref\":\"#/components/schemas/DemandsOutputDto\"}},\"application/json\":{\"schema\":{\"$ref\":\"#/components/schemas/DemandsOutputDto\"}},\"text/json\":{\"schema\":{\"$ref\":\"#/components/schemas/DemandsOutputDto\"}}}}}}},\"/Demands\":{\"post\":{\"tags\":[\"Demands\"],\"requestBody\":{\"content\":{\"application/json\":{\"schema\":{\"$ref\":\"#/components/schemas/DemandsCreateDto\"}},\"text/json\":{\"schema\":{\"$ref\":\"#/components/schemas/DemandsCreateDto\"}},\"application/*+json\":{\"schema\":{\"$ref\":\"#/components/schemas/DemandsCreateDto\"}}},\"required\":true},\"responses\":{\"200\":{\"description\":\"Success\",\"content\":{\"text/plain\":{\"schema\":{\"$ref\":\"#/components/schemas/DemandsOutputDto\"}},\"application/json\":{\"schema\":{\"$ref\":\"#/components/schemas/DemandsOutputDto\"}},\"text/json\":{\"schema\":{\"$ref\":\"#/components/schemas/DemandsOutputDto\"}}}}}}},\"/DemandsModule/ModifyState\":{\"put\":{\"tags\":[\"DemandsModule\"],\"parameters\":[{\"name\":\"id\",\"in\":\"query\",\"schema\":{\"type\":\"string\",\"format\":\"uuid\"}},{\"name\":\"state\",\"in\":\"query\",\"schema\":{\"$ref\":\"#/components/schemas/ModuleState\"}}],\"responses\":{\"200\":{\"description\":\"Success\",\"content\":{\"text/plain\":{\"schema\":{\"type\":\"boolean\"}},\"application/json\":{\"schema\":{\"type\":\"boolean\"}},\"text/json\":{\"schema\":{\"type\":\"boolean\"}}}}}}},\"/DemandsModule/{id}\":{\"get\":{\"tags\":[\"DemandsModule\"],\"parameters\":[{\"name\":\"id\",\"in\":\"path\",\"required\":true,\"schema\":{\"type\":\"string\",\"format\":\"uuid\"}}],\"responses\":{\"200\":{\"description\":\"Success\",\"content\":{\"text/plain\":{\"schema\":{\"$ref\":\"#/components/schemas/DemandsModuleOutputDto\"}},\"application/json\":{\"schema\":{\"$ref\":\"#/components/schemas/DemandsModuleOutputDto\"}},\"text/json\":{\"schema\":{\"$ref\":\"#/components/schemas/DemandsModuleOutputDto\"}}}}}},\"delete\":{\"tags\":[\"DemandsModule\"],\"parameters\":[{\"name\":\"id\",\"in\":\"path\",\"required\":true,\"schema\":{\"type\":\"string\",\"format\":\"uuid\"}}],\"responses\":{\"200\":{\"description\":\"Success\",\"content\":{\"text/plain\":{\"schema\":{\"type\":\"boolean\"}},\"application/json\":{\"schema\":{\"type\":\"boolean\"}},\"text/json\":{\"schema\":{\"type\":\"boolean\"}}}}}},\"put\":{\"tags\":[\"DemandsModule\"],\"parameters\":[{\"name\":\"id\",\"in\":\"path\",\"required\":true,\"schema\":{\"type\":\"string\",\"format\":\"uuid\"}}],\"requestBody\":{\"content\":{\"application/json\":{\"schema\":{\"$ref\":\"#/components/schemas/DemandsModuleModifiedDto\"}},\"text/json\":{\"schema\":{\"$ref\":\"#/components/schemas/DemandsModuleModifiedDto\"}},\"application/*+json\":{\"schema\":{\"$ref\":\"#/components/schemas/DemandsModuleModifiedDto\"}}},\"required\":true},\"responses\":{\"200\":{\"description\":\"Success\",\"content\":{\"text/plain\":{\"schema\":{\"$ref\":\"#/components/schemas/DemandsModuleOutputDto\"}},\"application/json\":{\"schema\":{\"$ref\":\"#/components/schemas/DemandsModuleOutputDto\"}},\"text/json\":{\"schema\":{\"$ref\":\"#/components/schemas/DemandsModuleOutputDto\"}}}}}}},\"/DemandsModule\":{\"post\":{\"tags\":[\"DemandsModule\"],\"requestBody\":{\"content\":{\"application/json\":{\"schema\":{\"$ref\":\"#/components/schemas/DemandsModuleCreateDto\"}},\"text/json\":{\"schema\":{\"$ref\":\"#/components/schemas/DemandsModuleCreateDto\"}},\"application/*+json\":{\"schema\":{\"$ref\":\"#/components/schemas/DemandsModuleCreateDto\"}}},\"required\":true},\"responses\":{\"200\":{\"description\":\"Success\",\"content\":{\"text/plain\":{\"schema\":{\"$ref\":\"#/components/schemas/DemandsModuleOutputDto\"}},\"application/json\":{\"schema\":{\"$ref\":\"#/components/schemas/DemandsModuleOutputDto\"}},\"text/json\":{\"schema\":{\"$ref\":\"#/components/schemas/DemandsModuleOutputDto\"}}}}}}}},\"components\":{\"schemas\":{\"DemandsCreateDto\":{\"required\":[\"description\",\"expectAccomplishDate\",\"modified\",\"name\",\"realityAccomplishDate\",\"state\",\"type\"],\"type\":\"object\",\"properties\":{\"name\":{\"type\":\"string\"},\"description\":{\"type\":\"string\"},\"type\":{\"$ref\":\"#/components/schemas/DemandType\"},\"state\":{\"$ref\":\"#/components/schemas/DemandState\"},\"expectAccomplishDate\":{\"type\":\"string\",\"format\":\"date-time\"},\"realityAccomplishDate\":{\"type\":\"string\",\"format\":\"date-time\"},\"modified\":{\"$ref\":\"#/components/schemas/DemandsModifiedDto\"}},\"additionalProperties\":false},\"DemandsModifiedDto\":{\"required\":[\"description\",\"expectAccomplishDate\",\"name\",\"realityAccomplishDate\",\"state\",\"type\"],\"type\":\"object\",\"properties\":{\"name\":{\"type\":\"string\"},\"description\":{\"type\":\"string\"},\"type\":{\"$ref\":\"#/components/schemas/DemandType\"},\"state\":{\"$ref\":\"#/components/schemas/DemandState\"},\"expectAccomplishDate\":{\"type\":\"string\",\"format\":\"date-time\"},\"realityAccomplishDate\":{\"type\":\"string\",\"format\":\"date-time\"}},\"additionalProperties\":false},\"DemandsModuleCreateDto\":{\"required\":[\"demandsId\",\"description\",\"name\",\"state\"],\"type\":\"object\",\"properties\":{\"demandsId\":{\"type\":\"string\",\"format\":\"uuid\"},\"name\":{\"type\":\"string\"},\"description\":{\"type\":\"string\"},\"state\":{\"$ref\":\"#/components/schemas/ModuleState\"}},\"additionalProperties\":false},\"DemandsModuleModifiedDto\":{\"required\":[\"demandsId\",\"description\",\"name\",\"state\"],\"type\":\"object\",\"properties\":{\"demandsId\":{\"type\":\"string\",\"format\":\"uuid\"},\"name\":{\"type\":\"string\"},\"description\":{\"type\":\"string\"},\"state\":{\"$ref\":\"#/components/schemas/ModuleState\"}},\"additionalProperties\":false},\"DemandsModuleOutputDto\":{\"type\":\"object\",\"properties\":{\"id\":{\"type\":\"string\",\"format\":\"uuid\"},\"createdBy\":{\"type\":\"string\",\"nullable\":true},\"modifiedBy\":{\"type\":\"string\",\"nullable\":true},\"createdDate\":{\"type\":\"string\",\"format\":\"date-time\"},\"modifiedDate\":{\"type\":\"string\",\"format\":\"date-time\"},\"isDeleted\":{\"type\":\"boolean\"},\"deletedBy\":{\"type\":\"string\",\"nullable\":true},\"deletedDate\":{\"type\":\"string\",\"format\":\"date-time\"},\"demandsId\":{\"type\":\"string\",\"format\":\"uuid\"},\"name\":{\"type\":\"string\",\"nullable\":true},\"description\":{\"type\":\"string\",\"nullable\":true},\"state\":{\"$ref\":\"#/components/schemas/ModuleState\"}},\"additionalProperties\":false},\"DemandsOutputDto\":{\"type\":\"object\",\"properties\":{\"id\":{\"type\":\"string\",\"format\":\"uuid\"},\"createdBy\":{\"type\":\"string\",\"nullable\":true},\"modifiedBy\":{\"type\":\"string\",\"nullable\":true},\"createdDate\":{\"type\":\"string\",\"format\":\"date-time\"},\"modifiedDate\":{\"type\":\"string\",\"format\":\"date-time\"},\"isDeleted\":{\"type\":\"boolean\"},\"deletedBy\":{\"type\":\"string\",\"nullable\":true},\"deletedDate\":{\"type\":\"string\",\"format\":\"date-time\"},\"name\":{\"type\":\"string\",\"nullable\":true},\"description\":{\"type\":\"string\",\"nullable\":true},\"type\":{\"$ref\":\"#/components/schemas/DemandType\"},\"state\":{\"$ref\":\"#/components/schemas/DemandState\"},\"expectAccomplishDate\":{\"type\":\"string\",\"format\":\"date-time\"},\"realityAccomplishDate\":{\"type\":\"string\",\"format\":\"date-time\"}},\"additionalProperties\":false},\"DemandState\":{\"enum\":[0,1,2,3,4,5,7,9,10],\"type\":\"integer\",\"format\":\"int32\"},\"DemandType\":{\"type\":\"integer\",\"format\":\"int32\"},\"ModuleState\":{\"type\":\"integer\",\"format\":\"int32\"}}}}";

        var swagger = data.ConvertJsonToObject<JObject>() ?? throw new NullReferenceException("swagger");

        ReSetPath(swagger, appid);
        // ReSetResponse(swagger);

        return new ContentResult
        {
            ContentType = "application/json",
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

    // TODO 这种方式没法兼容到 字节返回基础数据结构的接口
    private void ReSetResponse(JObject swagger)
    {
        try
        {
            var schemas = swagger["components"]?["schemas"];
            if (schemas == null) return;

            var tmp = new List<JProperty>();
            foreach (var jToken1 in schemas)
            {
                tmp.Add((JProperty) jToken1);
            }

            foreach (var item in tmp)
            {
                var name = item.Name;
                
                item.Replace(new JProperty($"{name}_Tmp", item.Value.DeepClone()));
                
                schemas[name] = JToken.Parse(new
                {
                    type = "object",
                    properties = new
                    {
                        success = new
                        {
                            type = "boolean"
                        },
                        result = new Dictionary<string, string>
                        {
                            {"type", "object"},
                            {"$ref", $"#/components/schemas/{name}_Tmp"},
                        },
                        requestId = new
                        {
                            type = "string"
                        }
                    }
                }.ConvertObjectToJson() ?? string.Empty);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "swagger get components.schemas error");
        }
    }

    [HttpGet("/proxy/{appid}/{**proxyMethod}")]
    [HttpPost("/proxy/{appid}/{**proxyMethod}")]
    [HttpPut("/proxy/{appid}/{**proxyMethod}")]
    [HttpDelete("/proxy/{appid}/{**proxyMethod}")]
    [HttpOptions("/proxy/{appid}/{**proxyMethod}")]
    [HttpHead("/proxy/{appid}/{**proxyMethod}")]
    [FormatResponse]
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
            throw new ProxyException($"Failed to invoke method {proxyMethod}", e);
        }

        var data = await response.Content.ReadAsStringAsync();
        if (response.StatusCode != HttpStatusCode.OK) throw new ProxyException($"Failed to {httpMethod} {proxyMethod} from {appid}", response.StatusCode, data);

        return new ContentResult
        {
            ContentType = response.Content.Headers.ContentType?.MediaType ?? "text/plain",
            Content = await response.Content.ReadAsStringAsync(),
            StatusCode = (int) HttpStatusCode.OK
        };
    }
}