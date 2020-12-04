using System.Web.Mvc;

namespace WEB.Areas.Contratos {
    public class ContratosAreaRegistration : AreaRegistration {
        public override string AreaName {
            get {
                return "Contratos";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) {

            context.MapRoute(
                name: "Contratos_Contrato",
                url: "contrato/{action}",
                defaults: new { controller = "contrato", AreaName = "Contratos" },
                namespaces: new[] { "WEB.Areas.Contratos.Controllers" }
            );

            context.MapRoute(
                "Contratos_default",
                "Contratos/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}