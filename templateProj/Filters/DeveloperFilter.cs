﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace templateProj.Filters
{
    public class DeveloperFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if ((filterContext.HttpContext.Session["Role"]).ToString() != "Developer")
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