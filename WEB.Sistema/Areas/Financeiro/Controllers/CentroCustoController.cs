using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Financeiro;
using WEB.Areas.Financeiro.ViewModels;
using DAL.Financeiro;
using PagedList;
using System.Json;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using BLL.Caches;
using MvcFlashMessages;
using WEB.App_Infrastructure;
using WEB.Helpers;
using WEB.ViewModels;

namespace WEB.Areas.Financeiro.Controllers {

    [OrganizacaoFilter]
    public class CentroCustoController : BaseSistemaController {

        //Atributos
        private IMacroContaBL _MacroContaBL;
        private ICentroCustoBL _CentroCustoBL;
        private ICentroCustoMacroContaBL _CentroCustoMacroContaBL;

        //Propriedades
        private IMacroContaBL OMacroContaBL => (this._MacroContaBL = this._MacroContaBL ?? new MacroContaBL());
        private ICentroCustoBL OCentroCustoBL => (this._CentroCustoBL = this._CentroCustoBL ?? new CentroCustoBL());
        private ICentroCustoMacroContaBL OCentroCustoMacroContaBL => (this._CentroCustoMacroContaBL = this._CentroCustoMacroContaBL ?? new CentroCustoMacroContaBL());

        //
        public ActionResult listar() {

            string descricao = UtilRequest.getString("valorBusca");

            bool? ativo = UtilRequest.getBool("flagAtivo");

            var lista = this.OCentroCustoBL.listar(descricao, ativo).OrderBy(x => x.descricao);

            return View(lista.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
        }

        //
        [HttpGet]
        public ActionResult editar(int? id) {

            var ViewModel = new CentroCustoForm();

            var OCentroCusto = this.OCentroCustoBL.carregar(UtilNumber.toInt32(id)) ?? new CentroCusto();

            ViewModel.listaMacroConta = this.OMacroContaBL.listar("", true).ToList();

            ViewModel.CentroCusto = OCentroCusto;

            ViewModel.carregarCheckboxes();

            return View(ViewModel);
        }

        //
        [HttpPost]
        public ActionResult editar(CentroCustoForm ViewModel) {

            ViewModel.listaMacroConta = this.OMacroContaBL.listar("", true).OrderBy(x => x.descricao).ToList();

            if (!ModelState.IsValid) {
                ViewModel.carregarCheckboxes();

                return View(ViewModel);
            }

            bool flagSucesso = this.OCentroCustoBL.salvar(ViewModel.CentroCusto);

            if (flagSucesso) {

                var idsSelecionados = ViewModel.listaSelecionados.Where(x => x.isChecked)
                    .Select(x => x.id)
                    .ToArray();

                var listaMacroContaCentroCusto = ViewModel.listaMacroConta
                    .Where(x => idsSelecionados.Contains(x.id))
                    .Select(o => new CentroCustoMacroConta() {idMacroConta = o.id, idCentroCusto = ViewModel.CentroCusto.id})
                    .ToList();

                this.OCentroCustoMacroContaBL.salvar(0, ViewModel.CentroCusto.id, listaMacroContaCentroCusto);
                
                CacheService.getInstance.remover(CentroCustoBL.keyCache);

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados foram salvos com sucesso."));

                return RedirectToAction("editar", new { ViewModel.CentroCusto.id });

            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));
            
            return View(ViewModel);
        }

        //
        [HttpPost]
        public ActionResult excluir(int[] id) {

            JsonMessage Retorno = new JsonMessage();

            Retorno.error = false;

            foreach (int idExclusao in id) {

                bool flagSucesso = this.OCentroCustoBL.excluir(idExclusao);

                if (!flagSucesso) {

                    Retorno.error = true;

                    Retorno.message = "Alguns registros não puderam ser excluídos.";
                }
            }

            Retorno.message = Retorno.message.isEmpty() ? "Os registros foram removidos com sucesso." : Retorno.message;

            CacheService.getInstance.remover(CentroCustoBL.keyCache);

            return Json(Retorno);
        }

        //
        [ActionName("alterar-status"), HttpPost]
        public ActionResult alterarStatus(int id) {

            var ORetorno = this.OCentroCustoBL.alterarStatus(id);

            if (!ORetorno.error) {
                CacheService.getInstance.remover(CentroCustoBL.keyCache);
            }
            
            return Json(ORetorno);
        }

        //
        [ActionName("gerar-excel"), HttpPost]
        public ActionResult gerarExcel() {

            List<int> ids = UtilRequest.getListInt("ids");

            List<CentroCusto> lista = this.OCentroCustoBL.listar("", null).Where(x => ids.Contains(x.id)).ToList();

            if (lista.Count > 0) {

                var listaExcel = lista.Select(x => new {
                    x.id,
                    x.descricao,
                    dtCadastro = x.dtCadastro.exibirData(),
                    status = (x.ativo == true) ? "Ativo" : "Desativado"
                }).ToList();

                var OGrid = new GridView();
                OGrid.DataSource = listaExcel;
                OGrid.DataBind();

                OGrid.HeaderRow.Cells[0].Text = "ID";
                OGrid.HeaderRow.Cells[1].Text = "Centro de Custo";
                OGrid.HeaderRow.Cells[2].Text = "Data de Cadastro";
                OGrid.HeaderRow.Cells[3].Text = "Status";

                UTIL.Excel.UtilExcel OExcel = new UTIL.Excel.UtilExcel();
                OExcel.downloadExcel(Response, OGrid, String.Concat("Lista de Centros de Custos - ", DateTime.Now.ToShortDateString().Replace("/", "-"), ".xls"));
            }

            return null;
        }

        [ActionName("listar-ajax")]
        public ActionResult listarAjax() {

            var query = this.OCentroCustoBL.listar("", true);

            var lista = query.Select(x => new { value = x.id, text = x.descricao }).Distinct().OrderBy(x => x.text).ToList();

            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        //
        [HttpGet, ActionName("modal-cadastrar-centro-custo")]
        public ActionResult modalCadastrarCentroCusto() {

            var ViewModel = new CentroCustoForm();

            ViewBag.modalId = UtilRequest.getString("modalId");
            ViewModel.group = UtilRequest.getString("group");
            ViewModel.CentroCusto.descricao = UtilRequest.getString("descricao");
            ViewModel.listaMacroConta = this.OMacroContaBL.listar("", true).ToList();
            
            foreach (var OMacroConta in ViewModel.listaMacroConta) {
                var Item = new CheckBoxItens { id = OMacroConta.id, isChecked = false, descricao = OMacroConta.descricao };
                ViewModel.listaSelecionados.Add(Item);
            }
            ViewModel.listaSelecionados = ViewModel.listaSelecionados.OrderBy(x => x.descricao).ToList();

            return View(ViewModel);
        }

        //
        [HttpPost, ActionName("salvar-centro-custo")]
        public ActionResult salvarCentroCusto(CentroCustoForm ViewModel) {

            ViewModel.listaMacroConta = this.OMacroContaBL.listar("", true).ToList();

            if (!ModelState.IsValid) {
                foreach (var OMacroConta in ViewModel.listaMacroConta) {
                    var Item = new CheckBoxItens { id = OMacroConta.id, isChecked = false, descricao = OMacroConta.descricao};
                    ViewModel.listaSelecionados.Add(Item);
                }
                ViewModel.listaSelecionados = ViewModel.listaSelecionados.OrderBy(x => x.descricao).ToList();

                return View("modal-cadastrar-centro-custo", ViewModel);
            }

            bool flagSucesso = this.OCentroCustoBL.salvar(ViewModel.CentroCusto);

            if (flagSucesso) {

                var idsSelecionados = ViewModel.listaSelecionados.Where(x => x.isChecked)
                    .Select(x => x.id).ToArray();

                var listaMacroContaCentroCusto = ViewModel.listaMacroConta
                    .Where(x => idsSelecionados.Contains(x.id))
                    .Select(o => new CentroCustoMacroConta() { idMacroConta = o.id, idCentroCusto = ViewModel.CentroCusto.id }).ToList();

                this.OCentroCustoMacroContaBL.salvar(0, ViewModel.CentroCusto.id, listaMacroContaCentroCusto);

                CacheService.getInstance.remover(CentroCustoBL.keyCache);

                return Json(new {error = false, ViewModel.CentroCusto.id, ViewModel.CentroCusto.descricao, ViewModel.group });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));

            return View("modal-cadastrar-centro-custo", ViewModel);
        }
    }
}