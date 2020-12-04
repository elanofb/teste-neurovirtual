using System;
using System.Web.Mvc;
using BLL.Financeiro;
using MvcFlashMessages;
using WEB.Areas.Financeiro.ViewModels;

namespace WEB.Areas.Financeiro.Controllers {

    public class TituloReceitaPagamentoExclusaoController : Controller {

        //Atributos
        private ITituloReceitaPagamentoBL _TituloReceitaPagamentoBL;

        //Propriedades
        private ITituloReceitaPagamentoBL OTituloReceitaPagamentoBL => _TituloReceitaPagamentoBL = _TituloReceitaPagamentoBL ?? new TituloReceitaPagamentoBL();

		/// <summary>
        /// Abrir o modal com formulario para registro da data de pagamento
        /// </summary>
		[HttpGet, ActionName("modal-excluir-pagamento")]
        public ActionResult modalExcluirPagamento(int id) {

		    var OPagamento = this.OTituloReceitaPagamentoBL.carregar(id);

            var ViewModel = new TituloReceitaExclusaoPagamentoForm();

            ViewModel.TituloReceitaPagamento = OPagamento;

			return PartialView(ViewModel);
        }

		/// <summary>
        /// Salvar o pagamento de uma parcela com baixa manual
        /// </summary>
		[HttpPost, ActionName("salvar-exclusao-pagamento")]
        public ActionResult salvarExclusaoPagamento(TituloReceitaExclusaoPagamentoForm ViewModel){

			if (!ModelState.IsValid) {
				
				ViewModel.TituloReceitaPagamento = this.OTituloReceitaPagamentoBL.carregar(ViewModel.TituloReceitaPagamento.id);

				return PartialView("modal-excluir-pagamento", ViewModel);
			}

		    //this.OTituloReceitaPagamentoBL.excluirPagamento(ViewModel.TituloReceitaPagamento, User.id());

			this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "A exclusão do pagamento foi realizada com sucesso.");

			return Json(new {error = false, message=""});

        }
    }
}