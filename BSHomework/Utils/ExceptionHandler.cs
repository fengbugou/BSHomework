using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BSHomework.Exceptions;

public class ExceptionHandler : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is HttpResponseException)
        {
            // 如果发生异常，将异常信息转为JSON格式并返回给客户端  
            context.Result = new JsonResult(new { error = context.Exception.Message });
            context.HttpContext.Response.StatusCode = ((HttpResponseException)context.Exception).Status;
            // context.HttpContext.Response.Body = new JsonResult(new { error = context.Exception.Message }).ToString();
        }
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
    }
}