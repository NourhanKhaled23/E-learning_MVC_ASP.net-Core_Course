using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using WebApplication1.Models;

namespace WebApplication1.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;
        private readonly IModelMetadataProvider _modelMetadataProvider;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger, IModelMetadataProvider modelMetadataProvider)
        {
            _logger = logger;
            _modelMetadataProvider = modelMetadataProvider;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Unhandled exception occurred: {Message}", context.Exception.Message);

            var result = new ViewResult
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(_modelMetadataProvider, context.ModelState)
                {
                    Model = new ErrorViewModel 
                    { 
                        RequestId = context.HttpContext.TraceIdentifier,
                        ExceptionMessage = context.Exception.Message
                    }
                }
            };
            

            context.Result = result;
            context.ExceptionHandled = true;
        }
    }
}
