using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CompanyEmployees.Presentation.ActionFilters
{
    public class ValidationFilterAttribute : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];

            var param = context.ActionArguments.SingleOrDefault(a => a.Value.ToString().Contains("dto")).Value;
            if(param == null)
            {
                context.Result = new BadRequestObjectResult($"Object is null. controller: {controller}, action: {action}");
                return;
            }

            if (!context.ModelState.IsValid) {
                context.Result = new UnprocessableEntityObjectResult(context.ModelState);
            }
        }
    }
}
