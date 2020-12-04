using System.Web.Mvc;

namespace WEB.Areas.ContasBancarias
{
    public class ContasBancariasAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ContasBancarias";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ContasBancarias_default",
                "ContasBancarias/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}