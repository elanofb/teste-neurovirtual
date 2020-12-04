using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.Financeiro;
using WEB.Areas.Financeiro.ViewModels;
using DAL.Financeiro;
using PagedList;
using System.Json;
using System.Web.UI.WebControls;
using BLL.Caches;
using MvcFlashMessages;
using WEB.App_Infrastructure;
using WEB.ViewModels;

namespace WEB.Areas.Financeiro.Controllers {

    [OrganizacaoFilter]
    public class MacroContaController : BaseSistemaController {

        //Atributos        
        private IMacroContaBL _MacroContaBL;
        private ICentroCustoMacroContaBL _CentroCustoMacroContaBL;
        private ICentroCustoBL _CentroCustoBL;
        private ITituloDespesaBL _TituloDespesaBL;
        private ITituloReceitaBL _TituloReceitaBL;
        private ITituloDespesaPagamentoBL _TituloDespesaPagamentoBL;
        private ITituloReceitaPagamentoBL _TituloReceitaPagamentoBL;

        //Propriedades
        private IMacroContaBL OMacroContaBL => this._MacroContaBL = this._MacroContaBL ?? new MacroContaBL();
        private ICentroCustoMacroContaBL OCentroCustoMacroContaBL => this._CentroCustoMacroContaBL = this._CentroCustoMacroContaBL ?? new CentroCustoMacroContaBL();
        private ICentroCustoBL OCentroCustoBL => this._CentroCustoBL = this._CentroCustoBL ?? new CentroCustoBL();
        private ITituloDespesaBL OTituloDespesaBL => _TituloDespesaBL = _TituloDespesaBL ?? new TituloDespesaPadraoBL();
        private ITituloReceitaBL OTituloReceitaBL => _TituloReceitaBL = _TituloReceitaBL ?? new TituloReceitaPadraoBL();
        private ITituloDespesaPagamentoBL OTituloDespesaPagamentoBL => _TituloDespesaPagamentoBL = _TituloDespesaPagamentoBL ?? new TituloDespesaPagamentoBL();
        private ITituloReceitaPagamentoBL OTituloReceitaPagamentoBL => _TituloReceitaPagamentoBL = _TituloReceitaPagamentoBL ?? new TituloReceitaPagamentoBL();

        //
        public ActionResult listar() {

            var descricao = UtilRequest.getString("valorBusca");
            var ativo = UtilRequest.getBool("flagAtivo");

            var listaMacroConta = this.OMacroContaBL.listar(descricao, ativo).OrderBy(x => x.descricao);

            return View(listaMacroConta.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
        }

        //
        [HttpGet]
        public ActionResult editar(int? id) {

            var ViewModel = new MacroContaForm();
            var OMacroConta = this.OMacroContaBL.carregar(UtilNumber.toInt32(id)) ?? new MacroConta();

            ViewModel.listaCentroCusto = this.OCentroCustoBL.listar("", true).OrderBy(x => x.descricao).ToList();

            ViewModel.MacroConta = OMacroConta;

            ViewModel.carregarCheckboxes();

            return View(ViewModel);
        }

        //
        [HttpPost]
        public ActionResult editar(MacroContaForm ViewModel) {

            ViewModel.listaCentroCusto = this.OCentroCustoBL.listar("", true).ToList();

            if (!ModelState.IsValid) {
                ViewModel.carregarCheckboxes();

                return View(ViewModel);
            }

            bool flagSucesso = this.OMacroContaBL.salvar(ViewModel.MacroConta);

            if (flagSucesso) {

                var idsSelecionados = ViewModel.listaSelecionados.Where(x => x.isChecked)
                    .Select(x => x.id)
                    .ToArray();

                var listaMacroContaCentroCusto = ViewModel.listaCentroCusto
                    .Where(x => idsSelecionados.Contains(x.id))
                    .Select(o => new CentroCustoMacroConta() { idMacroConta = ViewModel.MacroConta.id, idCentroCusto = o.id })
                    .ToList();

                this.OCentroCustoMacroContaBL.salvar(ViewModel.MacroConta.id, 0, listaMacroContaCentroCusto);

                CacheService.getInstance.limparCacheOrganizacao(null, MacroContaBL.keyCache);

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados foram salvos com sucesso."));

                return RedirectToAction("editar", new { ViewModel.MacroConta.id });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));

            return View(ViewModel);

        }

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
        [ActionName("alterar-status"), HttpPost]
        public ActionResult alterarStatus(int id) {

            var ORetorno = this.OMacroContaBL.alterarStatus(id);

            if (!ORetorno.error) {
                CacheService.getInstance.limparCacheOrganizacao(null, MacroContaBL.keyCache);
            }
            
            return Json(ORetorno);
        }

        //
        [ActionName("gerar-excel"), HttpPost]
        public ActionResult gerarExcel() {

            List<int> ids = UtilRequest.getListInt("ids");

            List<MacroConta> lista = this.OMacroContaBL.listar("", true).Where(x => ids.Contains(x.id)).ToList();

            if (lista.Count > 0) {

                var listaExcel = lista.Select(x => new {
                    x.id,
                    x.descricao,
                    dtCadastro = UtilDate.toDisplay(x.dtCadastro.ToString()),
                    status = x.ativo == true ? "Ativo" : "Desativado"
                }).ToList();

                var OGrid = new GridView();
                OGrid.DataSource = listaExcel;
                OGrid.DataBind();

                OGrid.HeaderRow.Cells[0].Text = "ID";
                OGrid.HeaderRow.Cells[1].Text = "Macro Conta";
                OGrid.HeaderRow.Cells[2].Text = "Data de Cadastro";
                OGrid.HeaderRow.Cells[3].Text = "Status";

                UTIL.Excel.UtilExcel OExcel = new UTIL.Excel.UtilExcel();
                OExcel.downloadExcel(Response, OGrid, String.Concat("Lista de Macro Contas - ", DateTime.Now.ToShortDateString().Replace("/", "-"), ".xls"));
            }

            return null;
        }

        //
        [ActionName("listar-ajax")]
        public ActionResult listarAjax(int? idCentroCusto, string flagReceitaDespesa) {

            var query = this.OMacroContaBL.listar("", true, UtilNumber.toInt32(idCentroCusto));

            if (!String.IsNullOrEmpty(flagReceitaDespesa)) {
                query = query.Where(x => x.flagReceitaDespesa == flagReceitaDespesa || x.flagReceitaDespesa == "A");
            }

            var lista = query.Select(x => new { value = x.id, text = x.descricao }).Distinct().OrderBy(x => x.text).ToList();

            return Json(lista, JsonRequestBehavior.AllowGet);
        }


        //
        [HttpGet, ActionName("modal-cadastrar-macro-conta")]
        public ActionResult modalCadastrarMacroConta() {

            var ViewModel = new MacroContaForm();

            var id = UtilRequest.getInt32("id");

            if (id > 0) {
                ViewModel.MacroConta = OMacroContaBL.carregar(id) ?? new MacroConta();
            }

            ViewModel.listaCentroCusto = this.OCentroCustoBL.listar("", true).OrderBy(x => x.descricao).ToList();
            ViewBag.modalId = UtilRequest.getString("modalId");
            ViewModel.group = UtilRequest.getString("group");

            if (ViewModel.MacroConta.id > 0) {
                var listIdsSelected = OCentroCustoMacroContaBL.listar(ViewModel.MacroConta.id, 0).Select(x => x.idCentroCusto).ToList();

                foreach (var OCentroCusto in ViewModel.listaCentroCusto) {
                    var Item = new CheckBoxItens { id = OCentroCusto.id, isChecked = listIdsSelected.Contains(OCentroCusto.id), descricao = OCentroCusto.descricao };
                    ViewModel.listaSelecionados.Add(Item);
                }
                ViewModel.listaSelecionados = ViewModel.listaSelecionados.OrderBy(x => x.descricao).ToList();
                
                return View(ViewModel);
            }

            ViewModel.MacroConta.descricao = UtilRequest.getString("descricao");
            var idCentroCusto = UtilRequest.getInt32("idCentroCusto");
                
            foreach (var OCentroCusto in ViewModel.listaCentroCusto) {
                var Item = new CheckBoxItens { id = OCentroCusto.id, isChecked = OCentroCusto.id == idCentroCusto, descricao = OCentroCusto.descricao };
                ViewModel.listaSelecionados.Add(Item);
            }
            ViewModel.listaSelecionados = ViewModel.listaSelecionados.OrderBy(x => x.descricao).ToList();
            
            return View(ViewModel);
        }

        //
        [HttpPost, ActionName("salvar-macro-conta")]
        public ActionResult salvarMacroConta(MacroContaForm ViewModel) {

            ViewModel.listaCentroCusto = this.OCentroCustoBL.listar("", true).ToList();

            if (!ModelState.IsValid) {
                foreach (var OCentroCusto in ViewModel.listaCentroCusto) {
                    var Item = new CheckBoxItens { id = OCentroCusto.id, isChecked = false, descricao = OCentroCusto.descricao };
                    ViewModel.listaSelecionados.Add(Item);
                }
                ViewModel.listaSelecionados = ViewModel.listaSelecionados.OrderBy(x => x.descricao).ToList();

                return View("modal-cadastrar-macro-conta", ViewModel);
            }

            bool flagSucesso = this.OMacroContaBL.salvar(ViewModel.MacroConta);

            if (flagSucesso) {

                var idsSelecionados = ViewModel.listaSelecionados.Where(x => x.isChecked)
                    .Select(x => x.id).ToArray();

                var listaMacroContaCentroCusto = ViewModel.listaCentroCusto
                    .Where(x => idsSelecionados.Contains(x.id))
                    .Select(o => new CentroCustoMacroConta() { idMacroConta = ViewModel.MacroConta.id, idCentroCusto = o.id })
                    .ToList();

                this.OCentroCustoMacroContaBL.salvar(ViewModel.MacroConta.id, 0, listaMacroContaCentroCusto);

                CacheService.getInstance.limparCacheOrganizacao(null, MacroContaBL.keyCache);

                return Json(new { error = false, ViewModel.MacroConta.id, ViewModel.MacroConta.descricao, ViewModel.group });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));

            return View("modal-cadastrar-macro-conta", ViewModel);
        }

        //
        [HttpGet, ActionName("modal-excluir-macro-conta")]
        public ActionResult modalExcluirMacroConta() {

            MacroContaVM ViewModel = new MacroContaVM();

            var id = UtilRequest.getInt32("id");

            if (id > 0) {
                ViewModel.qtdItens = OTituloDespesaBL.listar("").Count(x => x.idMacroConta == id) + OTituloReceitaBL.listar(0, 0, 0, "").Count(x => x.idMacroConta == id);
                ViewModel.qtdItens += OTituloDespesaPagamentoBL.listar(0).Count(x => x.idMacroConta == id) + OTituloReceitaPagamentoBL.listar(0).Count(x => x.idMacroConta == id);
            }

            ViewModel.idMacroConta = id;

            return View(ViewModel);
        }

        //
        [HttpPost, ActionName("excluir-macro-conta")]
        public ActionResult excluirMacroConta(MacroContaVM ViewModel) {
            
            var listaDespesas = OTituloDespesaBL.listar("").Where(x => x.idMacroConta == ViewModel.idMacroConta);
            var listaReceitas = OTituloReceitaBL.listar(0, 0, 0, "").Where(x => x.idMacroConta == ViewModel.idMacroConta);
            var listaDespesasPagamentos = OTituloDespesaPagamentoBL.listar(0).Where(x => x.idMacroConta == ViewModel.idMacroConta);
            var listaReceitasPagamentos = OTituloReceitaPagamentoBL.listar(0).Where(x => x.idMacroConta == ViewModel.idMacroConta);

            if (ViewModel.idMacroContaNova <= 0) {
                ViewModel.qtdItens = listaDespesas.Count() + listaReceitas.Count();
                ViewModel.qtdItens += listaDespesasPagamentos.Count() + listaReceitasPagamentos.Count();

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