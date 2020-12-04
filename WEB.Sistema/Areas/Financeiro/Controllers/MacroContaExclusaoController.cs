using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Financeiro;
using WEB.Areas.Financeiro.ViewModels;
using System.Json;
using BLL.Caches;
using MvcFlashMessages;
using WEB.App_Infrastructure;

namespace WEB.Areas.Financeiro.Controllers {

    [OrganizacaoFilter]
    public class MacroContaExclusaoController : BaseSistemaController {

        //Atributos        
        private IMacroContaBL _MacroContaBL;
        private ITituloDespesaBL _TituloDespesaBL;
        private ITituloReceitaBL _TituloReceitaBL;
        private ITituloDespesaPagamentoBL _TituloDespesaPagamentoBL;
        private ITituloReceitaPagamentoBL _TituloReceitaPagamentoBL;

        //Propriedades
        private IMacroContaBL OMacroContaBL => this._MacroContaBL = this._MacroContaBL ?? new MacroContaBL();
        private ITituloDespesaBL OTituloDespesaBL => _TituloDespesaBL = _TituloDespesaBL ?? new TituloDespesaPadraoBL();
        private ITituloReceitaBL OTituloReceitaBL => _TituloReceitaBL = _TituloReceitaBL ?? new TituloReceitaPadraoBL();
        private ITituloDespesaPagamentoBL OTituloDespesaPagamentoBL => _TituloDespesaPagamentoBL = _TituloDespesaPagamentoBL ?? new TituloDespesaPagamentoBL();
        private ITituloReceitaPagamentoBL OTituloReceitaPagamentoBL => _TituloReceitaPagamentoBL = _TituloReceitaPagamentoBL ?? new TituloReceitaPagamentoBL();
        
        //
        [HttpPost]
        public ActionResult excluir(int[] id) {

            var Retorno = new JsonMessage();
            Retorno.error = false;
            
            foreach (int idExclusao in id) {
                bool flagSucesso = this.OMacroContaBL.excluir(idExclusao);

                if (!flagSucesso) {
                    Retorno.error = true;
                    Retorno.message = "Alguns registros não puderam ser excluídos.";
                }
            }

            Retorno.message = Retorno.message.isEmpty() ? "Os registros foram removidos com sucesso." : Retorno.message;

            CacheService.getInstance.limparCacheOrganizacao(null, MacroContaBL.keyCache);

            return Json(Retorno);
        }
        
        //
        [HttpGet, ActionName("modal-excluir-macro-conta")]
        public ActionResult modalExcluirMacroConta() {

            MacroContaExclusaoVM ViewModel = new MacroContaExclusaoVM();

            var id = UtilRequest.getInt32("id");

            ViewModel.carregarDados(id);

            return View(ViewModel);
        }

        //
        [HttpPost, ActionName("excluir-macro-conta")]
        public ActionResult excluirMacroConta(MacroContaExclusaoVM ViewModel) {

            ViewModel.carregarDados(ViewModel.idMacroConta);

            var listaDespesas = OTituloDespesaBL.listar("").Where(x => x.idMacroConta == ViewModel.idMacroConta);
            var listaReceitas = OTituloReceitaBL.listar(0, 0, 0, "").Where(x => x.idMacroConta == ViewModel.idMacroConta);
            var listaDespesasPagamentos = OTituloDespesaPagamentoBL.listar(0).Where(x => x.idMacroConta == ViewModel.idMacroConta);
            var listaReceitasPagamentos = OTituloReceitaPagamentoBL.listar(0).Where(x => x.idMacroConta == ViewModel.idMacroConta);

            if (ViewModel.idMacroContaNova <= 0) {
                if (ViewModel.qtdItens > 0) {
                    this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Por favor preencha o campo."));

                    return View("modal-excluir-macro-conta", ViewModel);
                }
                
            }

            var Retorno = UtilRetorno.newInstance(false);

            if (listaDespesas.Any()) {
                Retorno = this.OTituloDespesaBL.substituirMacroConta(listaDespesas.Select(x => x.id).ToList(), ViewModel.idMacroContaNova);
            }

            if (listaReceitas.Any()) {
                Retorno = this.OTituloReceitaBL.substituirMacroConta(listaReceitas.Select(x => x.id).ToList(), ViewModel.idMacroContaNova);
            }

            if (listaDespesasPagamentos.Any()) {
                Retorno = this.OTituloDespesaPagamentoBL.substituirMacroConta(listaDespesasPagamentos.Select(x => x.id).ToList(), ViewModel.idMacroContaNova);
            }

            if (listaReceitasPagamentos.Any()) {
                Retorno = this.OTituloReceitaPagamentoBL.substituirMacroConta(listaReceitasPagamentos.Select(x => x.id).ToList(), ViewModel.idMacroContaNova);
            }

            if (!Retorno.flagError) {

                var flagSucesso = OMacroContaBL.excluir(ViewModel.idMacroConta);

                if (flagSucesso) {
                    return Json(new { error = false });
                }

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao excluir o registro. Tente novamente."));

                return View("modal-excluir-macro-conta", ViewModel);
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao excluir o registro. Tente novamente."));

            return View("modal-excluir-macro-conta", ViewModel);

        }
    }
}