using System.Web.Mvc;

namespace WEB.Areas.CuponsDesconto {
    public class CuponsDescontoAreaRegistration : AreaRegistration {
        public override string AreaName {
            get {
                return "CuponsDesconto";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) {
            context.MapRoute(
                "CuponsDesconto_default",
                "CuponsDesconto/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}