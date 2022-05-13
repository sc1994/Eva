using Eva.ToolKit.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Eva.ToolKit.Middlewares;

public class ThrowFriendlyException : IMiddleware
{
    public async Task InvokeAsync(HttpContext ctx, RequestDelegate next)
    {
        try
        {
            await next(ctx);
        }
        catch (BusinessException exception)
        {
            await WriteExceptionAsync(ctx, exception, exception.Title, exception.Code, exception.SubCode);
        }
        catch (Exception exception)
        {
            await WriteExceptionAsync(ctx, exception, "Internal error", "500");
        }
    }

    private async Task WriteExceptionAsync(HttpContext cxt, Exception exception, string title, string code, string? subCode = null)
    {
        cxt.Response.ContentType = AppConst.ContentJson;

        await cxt.Response.WriteAsync(JsonConvert.SerializeObject(new BadResponse()
        {
            Code = code,
            SubCode = subCode,
            Title = title,
            Message = exception.Message,
#if DEBUG
            Exception = exception,
#endif
            RequestId = cxt.TraceIdentifier
        }));
    }
}