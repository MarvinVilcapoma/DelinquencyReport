using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ArtSolutions.MUN.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            System.Web.Optimization.BundleTable.EnableOptimizations = true;
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.UserData;
            ModelBinders.Binders.Add(typeof(DateTime), new CustomDateModelBinder());
            ModelBinders.Binders.Add(typeof(DateTime?), new CustomDateModelBinder());
            //ModelBinders.Binders.Add(typeof(decimal), new DecimalBinder());
            //////Handle the null decimal type seperately 
            //ModelBinders.Binders.Add(typeof(decimal?), new DecimalBinder());
            ModelBinders.Binders.Add(typeof(decimal), new CustomDecimalModelBinder());
            ModelBinders.Binders.Add(typeof(decimal?), new CustomDecimalModelBinder());
        }
        protected void Application_BeginRequest()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
        }
    }
    public class CustomDateModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (value != null)
            {
                DateTime date;
                if ((!string.IsNullOrEmpty(UserSession.Current.Culture)) && DateTime.TryParse(value.AttemptedValue, CultureInfo.GetCultureInfo(UserSession.Current.Culture), DateTimeStyles.None, out date))
                {
                    return date;
                }
                else
                {
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, string.Format("{0} is an invalid date format", value.AttemptedValue));
                }
            }

            return base.BindModel(controllerContext, bindingContext);
        }
    }

    public class CustomDecimalModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (value != null)
            {
                CultureInfo ci = new CultureInfo(UserSession.Current.Culture);
                if (UserSession.Current.CountryID == 52)
                {
                    ci.NumberFormat.CurrencyDecimalDigits = UserSession.Current.DecimalPoints;
                    ci.NumberFormat.PercentDecimalDigits = UserSession.Current.DecimalPoints;
                    ci.NumberFormat.CurrencyDecimalSeparator = ci.NumberFormat.NumberDecimalSeparator = ci.NumberFormat.PercentDecimalSeparator = ".";
                    ci.NumberFormat.CurrencyGroupSeparator = ci.NumberFormat.NumberGroupSeparator = ci.NumberFormat.PercentGroupSeparator = ",";
                }

                decimal decimalValue;
                if ((!string.IsNullOrEmpty(UserSession.Current.Culture)) && decimal.TryParse(value.AttemptedValue, NumberStyles.Number | NumberStyles.AllowCurrencySymbol, ci, out decimalValue))
                {
                    return decimalValue;
                }
                else
                {
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, string.Format("{0} is an invalid date format", value.AttemptedValue));
                }
            }

            return base.BindModel(controllerContext, bindingContext);
        }
    }
}
