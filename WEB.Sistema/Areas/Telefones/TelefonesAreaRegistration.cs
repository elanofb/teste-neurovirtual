using System.Web.Mvc;

namespace WEB.Areas.Telefones {
    public class TelefonesAreaRegistration : AreaRegistration {

        public override string AreaName {
            get {
                return "Telefones";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) {
            context.MapRoute(
                "Telefones_default",
                "Telefones/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}