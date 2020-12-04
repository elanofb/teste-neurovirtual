using System.Web.Mvc;

namespace WEB.Areas.AssociadosConsultas
{
    public class AssociadosConsultasAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AssociadosConsultas";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AssociadosConsultas_default",
                "AssociadosConsultas/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}