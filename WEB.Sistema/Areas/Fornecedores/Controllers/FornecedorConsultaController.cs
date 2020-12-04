using System;
using System.Web.Mvc;
using BLL.Fornecedores;
using WEB.Areas.Fornecedores.ViewModels;
using WEB.Helpers;

namespace WEB.Areas.Fornecedores.Controllers {
		
	[OrganizacaoFilter]
	public class FornecedorConsultaController : Controller {

		//Constantes

		//Atributos
		private IFornecedorConsultaBL _FornecedorConsultaBL;

		//Propriedades
		private IFornecedorConsultaBL OFornecedorConsultaBL => _FornecedorConsultaBL = _FornecedorConsultaBL ?? new FornecedorConsultaBL();

		//Events

		//GET: 
		public ActionResult listar() {

            var ViewModel = new FornecedorVM();
			
			ViewModel.carregar();
			
            if (ViewModel.flagTipoSaida == TipoSaidaHelper.EXCEL) {

                var OFornecedorExportacao = new FornecedorExportacao();
                OFornecedorExportacao.baixarExcel(ViewModel.listaFornecedores);

                return null;
            }

            return View(ViewModel);
		}
		
		//GET: Autocompletar busca por fornecedor
		public ActionResult getFornecedor(string term, int? idFornecedor) {
			var listAssociados = this.OFornecedorConsultaBL.autocompletar(term, UtilNumber.toInt32(idFornecedor));
			return Json(listAssociados, JsonRequestBehavior.AllowGet);
		}
	}
}
