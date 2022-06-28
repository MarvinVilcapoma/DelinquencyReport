using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.ChatAndBot
{
    public class ChatAndBotAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ChatAndBot";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ChatAndBot_default",
                "ChatAndBot/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}