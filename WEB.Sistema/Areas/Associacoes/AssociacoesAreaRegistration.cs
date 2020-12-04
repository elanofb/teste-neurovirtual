using System.Web.Mvc;

namespace WEB.Areas.Associacoes {
    public class AssociacoesAreaRegistration : AreaRegistration {

        public override string AreaName {
            get {
                return "Associacoes";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) {
            context.MapRoute(
                "Associacoes_default",
                "Associacoes/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}