using System.Web.Mvc;

namespace WEB.Areas.FinanceiroLancamentos
{
    public class FinanceiroLancamentosAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "FinanceiroLancamentos";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "FinanceiroLancamentos_default",
                "FinanceiroLancamentos/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}