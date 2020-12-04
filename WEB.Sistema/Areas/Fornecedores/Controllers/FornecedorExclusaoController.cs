using System.Json;
using System.Web.Mvc;
using BLL.Fornecedores;

namespace WEB.Areas.Fornecedores.Controllers {

	public class FornecedorExclusaoController : Controller {

		//Constantes

		//Atributos
		private IFornecedorExclusaoBL _FornecedorExclusaoBL;

		//Propriedades
		private IFornecedorExclusaoBL OFornecedorExclusaoBL => _FornecedorExclusaoBL = _FornecedorExclusaoBL ?? new FornecedorExclusaoBL();

		//POST: 
		[HttpPost]
		public ActionResult excluir(int[] id) {
			var Retorno = new JsonMessage();
			Retorno.error = false;
			Retorno.message = "Os registros foram removidos com sucesso.";

			foreach (int idFornecedor in id) {

				var RetornoExclusao = this.OFornecedorExclusaoBL.excluir(idFornecedor);

				if (RetornoExclusao.flagError) {
					Retorno.error = true;
					Retorno.message = "Alguns registros não puderam ser removidos.";
				}
			}
			return Json(Retorno);
		}
		
	}
}
