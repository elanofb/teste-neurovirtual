using System.Web.Mvc;

namespace WEB.Areas.FinanceiroParcelamentos {
    public class FinanceiroParcelamentosAreaRegistration : AreaRegistration {
        public override string AreaName {
            get {
                return "FinanceiroParcelamentos";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) {
            context.MapRoute(
                "FinanceiroParcelamentos_default",
                "FinanceiroParcelamentos/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}