using System.Web.Mvc;

namespace WEB.Areas.ConfiguracoesEtiquetas {

    public class ConfiguracoesEtiquetasAreaRegistration : AreaRegistration {

        public override string AreaName => "ConfiguracoesEtiquetas";

        public override void RegisterArea(AreaRegistrationContext context) {

            context.MapRoute(
                "ConfiguracoesEtiquetas_default",
                "ConfiguracoesEtiquetas/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

        }

    }

}