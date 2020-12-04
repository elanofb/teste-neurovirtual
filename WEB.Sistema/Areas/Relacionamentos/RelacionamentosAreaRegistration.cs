using System.Web.Mvc;

namespace WEB.Areas.Relacionamentos {
    public class RelacionamentosAreaRegistration : AreaRegistration {
        public override string AreaName {
            get {
                return "Relacionamentos";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) {

            context.MapRoute(
                name: "Relacionamentos_Relacionamento",
                url: "relacionamento/{action}",
                defaults: new { controller = "relacionamento", AreaName = "Relacionamentos" },
                namespaces: new[] { "WEB.Areas.Relacionamentos.Controllers" }
            );

            context.MapRoute(
                "Relacionamentos_default",
                "Relacionamentos/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}