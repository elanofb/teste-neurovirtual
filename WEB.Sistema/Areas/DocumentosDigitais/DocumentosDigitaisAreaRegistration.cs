using System.Web.Mvc;

namespace WEB.Areas.DocumentosDigitais {
    public class DocumentosDigitaisAreaRegistration : AreaRegistration {

        public override string AreaName => "DocumentosDigitais";

        public override void RegisterArea(AreaRegistrationContext context) {
            context.MapRoute(
                "DocumentosDigitais_default",
                "DocumentosDigitais/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}