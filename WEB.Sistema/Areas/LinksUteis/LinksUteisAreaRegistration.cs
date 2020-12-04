using System.Web.Mvc;

namespace WEB.Areas.LinksUteis
{
    public class LinksUteisAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "LinksUteis";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "LinksUteis_default",
                "LinksUteis/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}