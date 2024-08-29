using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace EcommerceManagement.Crosscutting.Attribute
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        private readonly string _viewName;

        public ValidateModelAttribute(string viewName)
        {
            _viewName = viewName;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var controller = (Controller)context.Controller;
                var model = context.ActionArguments.Values.FirstOrDefault();
                context.Result = controller.View(_viewName, model);
            }
        }
    }
}
