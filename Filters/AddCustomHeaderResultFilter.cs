using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication1.Filters
{
    public class AddCustomHeaderResultFilter : IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Headers["X-Powered-By"] = "E-Learning-MVC-App";
            context.HttpContext.Response.Headers["X-App-Version"] = "1.0.0";
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {

        }
    }
}
