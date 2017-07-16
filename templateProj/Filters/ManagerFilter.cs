using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace templateProj.Filters
{
    public class ManagerFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Debug.WriteLine("2");

            if ((filterContext.HttpContext.Session["Role"]).ToString() 
                != "PManager")
            {

                filterContext.Result = new RedirectToRouteResult(
                 new RouteValueDictionary 
                { 
                    { "controller", "Session" }, 
                    { "action", "RestrictionError" } 
                });

            }
        }

    }
}