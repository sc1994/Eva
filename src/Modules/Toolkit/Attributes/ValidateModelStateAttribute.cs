using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Eva.ToolKit.Attributes;

public class ValidateModelStateAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext ctx)
    {
        if (ctx == null) throw new ArgumentNullException(nameof(ctx));
        if (ctx.ModelState.IsValid)
        {
            base.OnActionExecuting(ctx);
            return;
        }

        ctx.Result = new JsonResult(new BadResponse
        {
            Code = "400",
            SubCode = "001",
            Title = "请求参数验证不通过",
            Messages = ctx.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToArray()
        });
    }
}