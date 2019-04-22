using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Helpdesk.Website
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
           bool isDefined = filterContext.ActionDescriptor.IsDefined(typeof(AuthAttribute), true) ||
                       filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AuthAttribute), true);
            
            if (!isDefined)
                return;

            if (filterContext.HttpContext.Session["ThisUser"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Login" }));
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            
        }
    }
}


