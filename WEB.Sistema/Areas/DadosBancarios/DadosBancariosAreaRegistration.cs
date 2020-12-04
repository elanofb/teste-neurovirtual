using System.Web.Mvc;

namespace WEB.Areas.DadosBancarios{
    
    public class DadosBancariosAreaRegistration : AreaRegistration{
        
        public override string AreaName{
            get{ return "DadosBancarios"; }
        }
            
        public override void RegisterArea(AreaRegistrationContext context){
            context.MapRoute("DadosBancarios_default", "DadosBancarios/{controller}/{action}/{id}", new{action = "Index", id = UrlParameter.Optional});
        }        
        
    }
}