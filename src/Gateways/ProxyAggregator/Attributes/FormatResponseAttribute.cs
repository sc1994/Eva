using System.Net.Mime;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Eva.ProxyAggregator.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class FormatResponseAttribute : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext ctx)
    {
        if (ctx == null) throw new ArgumentNullException(nameof(ctx));
        if (ctx.Exception != null) return;

        ctx.Result = ctx.Result switch
        {
            ObjectResult objectResult =>
                new ObjectResult(FormatResponse(objectResult.Value, ctx.HttpContext.TraceIdentifier)),
            JsonResult jsonResult =>
                new JsonResult(FormatResponse(jsonResult.Value, ctx.HttpContext.TraceIdentifier)),
            ContentResult contentResult when contentResult.ContentType?.Contains("application/json") == true =>
                new ContentResult
                {
                    Content = FormatResponse(contentResult.Content, ctx.HttpContext.TraceIdentifier, contentResult.StatusCode).ConvertObjectToJson(),
                    ContentType = "application/json",
                    StatusCode = contentResult.StatusCode,
                },
            ContentResult contentResult when contentResult.ContentType?.Contains("text/plain") == true =>
                new ContentResult
                {
                    Content = FormatResponse(contentResult.Content, ctx.HttpContext.TraceIdentifier, contentResult.StatusCode == 200).ConvertObjectToJson(),
                    ContentType = "application/json",
                    StatusCode = contentResult.StatusCode,
                },
            _ => ctx.Result
        };

        base.OnActionExecuted(ctx);
    }

    private object FormatResponse(object? response, string traceIdentifier, bool success = true)
    {
        return new
        {
            success,
            result = response,
            requestId = traceIdentifier
        };
    }

    private object? FormatResponse(string? response, string traceIdentifier, int? statusCode)
    {
        return FormatResponse(response.ConvertJsonToObject(), traceIdentifier, statusCode == 200);
    }
}