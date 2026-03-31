using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication1.Filters
{
    public class StudentHeaderAuthorizationFilter : Attribute, IAuthorizationFilter
    {
        private readonly string _requiredValue;

        public StudentHeaderAuthorizationFilter(string requiredValue = "Student")
        {
            _requiredValue = requiredValue;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {

            var studentHeader = context.HttpContext.Request.Headers["Student"].ToString();
            var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString();

            if (studentHeader != _requiredValue && authHeader != _requiredValue)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
