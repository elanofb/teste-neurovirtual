using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BLL.Financeiro;
using DAL.Financeiro;
using PagedList;

namespace WEB.Areas.Financeiro.Controllers
{
    public class MacroContaConsultaController : Controller
    {

        //Atributos        
        private IMacroContaBL _MacroContaBL;

        //Propriedades
        private IMacroContaBL OMacroContaBL => this._MacroContaBL = this._MacroContaBL ?? new MacroContaBL();

        //
        public ActionResult listar() {

            var descricao = UtilRequest.getString("valorBusca");
            var ativo = UtilRequest.getBool("flagAtivo");

            var listaMacroConta = this.OMacroContaBL.listar(descricao, ativo).OrderBy(x => x.descricao);

            return View(listaMacroConta.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
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
    }
}