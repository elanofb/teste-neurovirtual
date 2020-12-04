using System.Web.Mvc;
using BLL.Notificacoes;

namespace WEB.Areas.PessoasDevices.Controllers {

	public class DispositivoExclusaoController : Controller {

		//Atributos
		private IPessoaDeviceExclusaoBL _IPessoaDeviceExclusaoBL;

		//Propriedades
		private IPessoaDeviceExclusaoBL OPessoaDeviceExclusaoBL => _IPessoaDeviceExclusaoBL = _IPessoaDeviceExclusaoBL ?? new PessoaDeviceExclusaoBL();
		
	    //
		public JsonResult excluir(int[] ids) {
			return Json(this.OPessoaDeviceExclusaoBL.excluir(ids), JsonRequestBehavior.AllowGet);
		}

	}
	
}
