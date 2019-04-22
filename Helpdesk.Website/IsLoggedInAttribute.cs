using System;
using System.Linq;
using System.Web.Mvc;
using Helpdesk.Repository;

namespace Helpdesk.Website
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class IsLoggedInAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["ThisUser"] != null)
            {
                var user = filterContext.HttpContext.Session["ThisUser"] as User;
                filterContext.Controller.ViewBag.LoggedIn = true;
                filterContext.Controller.ViewBag.Role = user.UserRoles.First().Role.Name;
                filterContext.Controller.ViewBag.BrandId = user.UserBrands.First().Brand.Id;
                filterContext.Controller.ViewBag.UserId = user.Id;
            }
            else
            {
                filterContext.Controller.ViewBag.LoggedIn = false;
                filterContext.Controller.ViewBag.Role = "None";
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