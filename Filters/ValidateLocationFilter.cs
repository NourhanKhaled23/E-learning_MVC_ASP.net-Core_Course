using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication1.Filters
{
    public class ValidateLocationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Models.Department? dept = null;

            if (context.ActionArguments.TryGetValue("department", out object? departmentObj) && departmentObj is Models.Department department)
            {
                dept = department;
            }

            if (dept != null)
            {
                var location = dept.Location;
                if (!string.Equals(location, "USA", StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(location, "EG", StringComparison.OrdinalIgnoreCase))
                {
                    var controller = context.Controller as Controller;
                    if (controller != null)
                    {
                        controller.ModelState.AddModelError("Location", "Invalid location. Only 'USA' or 'EG' are accepted.");
                        

                        context.Result = new ViewResult
                        {
                            ViewName = context.ActionDescriptor.RouteValues["action"], 
                            ViewData = controller.ViewData,
                            TempData = controller.TempData
                        };
                    }
                    else
                    {

                        context.Result = new BadRequestObjectResult("Invalid location. Only 'USA' or 'EG' are accepted.");
                    }
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
