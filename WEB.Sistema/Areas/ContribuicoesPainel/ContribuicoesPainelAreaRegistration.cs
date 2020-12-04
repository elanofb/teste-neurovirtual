using System.Web.Mvc;

namespace WEB.Areas.ContribuicoesPainel
{
    public class ContribuicoesPainelAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ContribuicoesPainel";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ContribuicoesPainel_default",
                "ContribuicoesPainel/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}