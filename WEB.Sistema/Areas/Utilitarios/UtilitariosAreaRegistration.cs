using System.Web.Mvc;

namespace WEB.Areas.Utilitarios {

    public class UtilitariosAreaRegistration : AreaRegistration {

        public override string AreaName {
            get { return "Utilitarios"; }
        }

        public override void RegisterArea(AreaRegistrationContext context) {

            context.MapRoute(
                "Utilitarios_default",
                "Utilitarios/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}