using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;


namespace templateProj.Filters
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple
        = false)]
    public class SessionTimeOutFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controllerName
                = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower();

            HttpSessionStateBase session = filterContext.HttpContext.Session;

            if (session.IsNewSession)
            {
                //Redirect user to the session timeout message
                var url = new UrlHelper(filterContext.RequestContext);
                var loginUrl = url.Content("/Session/Timeout");

                filterContext.HttpContext.Response.Redirect(loginUrl, true);
            }
        }

    }
}