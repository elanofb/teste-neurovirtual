using System.Web.Mvc;

namespace WEB.Areas.Popups {
    public class PopupsAreaRegistration : AreaRegistration {
        public override string AreaName {
            get {
                return "Popups";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) {
            context.MapRoute(
                "Popups_default",
                "Popups/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}