using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Financeiro;
using WEB.Areas.Financeiro.ViewModels;
using System.Json;
using MvcFlashMessages;

namespace WEB.Areas.Financeiro.Controllers {

    public class SubContaExclusaoController : Controller {
        //Constantes

        //Atributos
        private ICategoriaTituloBL _CategoriaTituloBL;
        private ITituloDespesaBL _TituloDespesaBL;
        private ITituloReceitaBL _TituloReceitaBL;
        private ITituloDespesaPagamentoBL _TituloDespesaPagamentoBL;
        private ITituloReceitaPagamentoBL _TituloReceitaPagamentoBL;

        //Propriedades
        private ICategoriaTituloBL OCategoriaTituloBL => _CategoriaTituloBL = _CategoriaTituloBL ?? new CategoriaTituloBL();
        private ITituloDespesaBL OTituloDespesaBL => _TituloDespesaBL = _TituloDespesaBL ?? new TituloDespesaPadraoBL();
        private ITituloReceitaBL OTituloReceitaBL => _TituloReceitaBL = _TituloReceitaBL ?? new TituloReceitaPadraoBL();
        private ITituloDespesaPagamentoBL OTituloDespesaPagamentoBL => _TituloDespesaPagamentoBL = _TituloDespesaPagamentoBL ?? new TituloDespesaPagamentoBL();
        private ITituloReceitaPagamentoBL OTituloReceitaPagamentoBL => _TituloReceitaPagamentoBL = _TituloReceitaPagamentoBL ?? new TituloReceitaPagamentoBL();
        
        [HttpPost]
        public ActionResult excluir(int[] id) {

            JsonMessage Retorno = new JsonMessage();

            Retorno.error = false;

            foreach (int idExclusao in id) {

                bool flagSucesso = this.OCategoriaTituloBL.excluir(idExclusao);

                if (!flagSucesso) {

                    Retorno.error = true;

                    Retorno.message = "Alguns registros não puderam ser excluídos.";
                }
            }

            Retorno.message = Retorno.message.isEmpty() ? "Os registros foram removidos com sucesso." : Retorno.message;

            return Json(Retorno);
        }
        
        //
        [HttpGet, ActionName("modal-excluir-sub-conta")]
        public ActionResult modalExcluirSubConta() {

            CategoriaTituloExclusaoVM ViewModel = new CategoriaTituloExclusaoVM();

            var id = UtilRequest.getInt32("id");

            ViewModel.carregarDados(id);

            ViewModel.idMacroContaNova = ViewModel.idMacroConta;
            
            return View(ViewModel);
        }

        //
        [HttpPost, ActionName("excluir-sub-conta")]
        public ActionResult excluirSubConta(CategoriaTituloExclusaoVM ViewModel) {

            ViewModel.carregarDados(ViewModel.idCategoria);

            var listaDespesas = OTituloDespesaBL.listar("").Where(x => x.idCategoria == ViewModel.idCategoria);
            var listaReceitas = OTituloReceitaBL.listar(0, 0, 0, "").Where(x => x.idCategoria == ViewModel.idCategoria);
            var listaDespesasPagamentos = OTituloDespesaPagamentoBL.listar(0).Where(x => x.idCategoria == ViewModel.idCategoria);
            var listaReceitasPagamentos = OTituloReceitaPagamentoBL.listar(0).Where(x => x.idCategoria == ViewModel.idCategoria);

            if (ViewModel.idCategoriaNova <= 0 || ViewModel.idMacroContaNova <= 0) {
                
                if (ViewModel.qtdItens > 0) {
                    this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Por favor preencha o campo."));

                    return View("modal-excluir-sub-conta", ViewModel);
                }
            }

            var Retorno = UtilRetorno.newInstance(false);

            if (listaDespesas.Any()) {
                Retorno = this.OTituloDespesaBL.substituirCategoriaEMacroConta(listaDespesas.Select(x => x.id).ToList(), ViewModel.idCategoriaNova, ViewModel.idMacroContaNova);
            }

            if (listaReceitas.Any()) {
                Retorno = this.OTituloReceitaBL.substituirCategoriaEMacroConta(listaReceitas.Select(x => x.id).ToList(), ViewModel.idCategoriaNova, ViewModel.idMacroContaNova);
            }

            if (listaDespesasPagamentos.Any()) {
                Retorno = this.OTituloDespesaPagamentoBL.substituirCategoriaEMacroConta(listaDespesasPagamentos.Select(x => x.id).ToList(), ViewModel.idCategoriaNova, ViewModel.idMacroContaNova);
            }

            if (listaReceitasPagamentos.Any()) {
                Retorno = this.OTituloReceitaPagamentoBL.substituirCategoriaEMacroConta(listaReceitasPagamentos.Select(x => x.id).ToList(), ViewModel.idCategoriaNova, ViewModel.idMacroContaNova);
            }

            if (!Retorno.flagError) {

                var flagSucesso = OCategoriaTituloBL.excluir(ViewModel.idCategoria);

                if (flagSucesso) {
                    return Json(new { error = false });
                }

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao excluir o registro. Tente novamente."));

                return View("modal-excluir-sub-conta", ViewModel);
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao excluir o registro. Tente novamente."));

            return View("modal-excluir-sub-conta", ViewModel);

        }
    }
}