using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Schedulers
{
    public class SchedulersAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Schedulers";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Schedulers_default",
                "Schedulers/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}