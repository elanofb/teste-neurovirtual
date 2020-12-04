using System;
using System.Web.Mvc;
using WEB.App_Infrastructure;

namespace WEB.Areas.RedeAfiliados.Controllers {

	public class RedeBinariaController : BaseSistemaController {

	    //Construtor
		public RedeBinariaController() { 
				
		}
        
		/// <summary>
		/// 
		/// </summary>
		public ActionResult index() {

			int idMembro = UtilRequest.getInt32("idMembro");

			if (idMembro == 0) {
				idMembro = 1;
			}
			
			return View();
		}

	}
}