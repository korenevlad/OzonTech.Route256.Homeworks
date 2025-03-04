using GoodsService.BLL.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GoodsService.Presentation.Middleware;
public class BusinessExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is BusinessException businessException)
        {
            var response = context.HttpContext.Response;
            response.StatusCode = 400;
            context.ExceptionHandled = true;
            context.Result = new ObjectResult(businessException.Message);
        }
    }
}