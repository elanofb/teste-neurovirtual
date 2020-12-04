using System;
using System.Web.Mvc;
using MvcFlashMessages;
using WEB.Areas.Financeiro.ViewModels;

namespace WEB.Areas.Financeiro.Controllers {
    public class TituloImpostoCadastroController : Controller {

        private ICalculadorImpostoTituloBL _CalculadorImpostoTituloBL;

        //Propriedades
        private ICalculadorImpostoTituloBL OCalculadorImpostoTituloBL => _CalculadorImpostoTituloBL = _CalculadorImpostoTituloBL ?? new CalculadorImpostoTituloBL();

        //
        [HttpGet, ActionName("modal-cadastrar-titulo-imposto")]
        public ActionResult modalCadastrarTituloImposto() {

            TituloImpostoForm ViewModel = new TituloImpostoForm();

            ViewModel.TituloImposto.idTituloReceita = UtilRequest.getInt32("idTituloReceita");
            ViewModel.TituloImposto.idTituloDespesa = UtilRequest.getInt32("idTituloDespesa");

            return View(ViewModel);
        }

        //
        [HttpPost, ActionName("salvar-titulo-imposto")]
        public ActionResult salvarMacroConta(TituloImpostoForm ViewModel) {

            if (!ModelState.IsValid) {
                return View("modal-cadastrar-titulo-imposto", ViewModel);
            }

            var Retorno = this.OCalculadorImpostoTituloBL.calcularImpostoTitulo(ViewModel.idTabelaImposto, ViewModel.TituloImposto);

            if (!Retorno.flagError) {
                return Json(new { error = false, message = Retorno.listaErros.FirstOrDefault() });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", Retorno.listaErros.FirstOrDefault()));

            return View("modal-cadastrar-titulo-imposto", ViewModel);

        }
    }
}