using System.Web.Mvc;

namespace WEB.Areas.PessoasDevices {
	
	public class PessoasDevicesAreaRegistration : AreaRegistration {
		
		public override string AreaName => "PessoasDevices";

		public override void RegisterArea(AreaRegistrationContext context) {

			context.MapRoute(
				"PessoasDevices_default",
				"PessoasDevices/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
			
		}
	}
}