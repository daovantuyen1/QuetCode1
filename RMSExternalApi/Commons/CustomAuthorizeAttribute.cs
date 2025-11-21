using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace RMSExternalApi.Commons
{
    public class CustomAuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            if (HttpContext.Current.Session != null && HttpContext.Current.Session["Account"] != null)
            {
                return true;
            }
            return false;

        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(
                new { controller = "Home", action = "Login" }
                ));

        }
    }
}