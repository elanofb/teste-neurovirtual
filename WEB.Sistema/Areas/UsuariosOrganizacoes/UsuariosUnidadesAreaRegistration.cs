using System.Web.Mvc;

namespace WEB.Areas.UsuariosUnidades {
    public class UsuariosUnidadesAreaRegistration : AreaRegistration {
        public override string AreaName {
            get {
                return "UsuariosUnidades";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) {

            context.MapRoute(
                "UsuariosUnidades_default",
                "UsuariosUnidades/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}