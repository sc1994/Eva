namespace Eva.ProxyAggregator.Middlewares;

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
            await WriteExceptionAsync(ctx, exception, exception.Title, $"501_{exception.ErrorCode}");
        }
        catch (ProxyException exception)
        {
            await WriteExceptionAsync(ctx, exception, exception.Title, $"510_{(int) exception.StatusCode}");
        }
        catch (Exception exception)
        {
            await WriteExceptionAsync(ctx, exception, "Internal error", "500_000");
        }
    }

    private async Task WriteExceptionAsync(HttpContext cxt, Exception exception, string title, string errorCode)
    {
        cxt.Response.ContentType = "application/json";

        await cxt.Response.WriteAsync(JsonConvert.SerializeObject(new
        {
            success = false,
            errorCode,
            title,
            message = exception.Message,
#if DEBUG
            stackTrace = exception.StackTrace,
#endif
            requestId = cxt.TraceIdentifier
        }));
    }
}