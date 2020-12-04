using System.Web.Mvc;

namespace WEB.Areas.Portais{
    public class PortaisAreaRegistration : AreaRegistration {
        public override string AreaName => "Portais";

        public override void RegisterArea(AreaRegistrationContext context) {
            context.MapRoute(
                "Portais_default",
                "Portais/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}