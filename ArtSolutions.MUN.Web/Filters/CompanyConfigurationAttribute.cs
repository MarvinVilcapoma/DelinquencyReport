using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ArtSolutions.MUN.Web.Filters
{
    public class IsCompanyConfiguredAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Check Company Exists Or New Registration
            if ((!UserSession.Current.IsCompanyConfigured) && (!UserSession.Current.IsOwner))
            {
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary{{ "controller", "Company" },
                                              { "action", "UnAuthorizeForRegistration" }
                                         });
            }
            //Check Company Exists Or New Registration
            else if (!UserSession.Current.IsCompanyConfigured)
            {
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary{{ "controller", "Company" },
                                              { "action", "Registration" }
                                         });
            }
           
            base.OnActionExecuting(filterContext);
        }
    }
}