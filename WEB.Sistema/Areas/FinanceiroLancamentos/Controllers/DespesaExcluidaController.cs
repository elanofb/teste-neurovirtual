using System;
using System.Linq;
using System.Web.Mvc;
using BLL.FinanceiroLancamentos;
using WEB.Areas.LancamentoRecebimentos.ViewModels;
using PagedList;
using WEB.App_Infrastructure;
using WEB.Helpers;
using WEB.Areas.FinanceiroLancamentos.ViewModels;

namespace WEB.Areas.FinanceiroLancamentos.Controllers
{
    public class DespesaExcluidaController : BaseSistemaController{
        //Atributos
        private ITituloDespesaPagamentoResumoVWBL _TituloDespesaPagamentoResumoVWBL;

        //Propriedades
        private ITituloDespesaPagamentoResumoVWBL OTituloDespesaPagamentoResumoVWBL => this._TituloDespesaPagamentoResumoVWBL = this._TituloDespesaPagamentoResumoVWBL ?? new TituloDespesaPagamentoResumoVWBL();

        ///
        public ActionResult listar(){
            var dtNow = DateTime.Now;
            var dtNowYear = dtNow.Year;
            var dtNowMonth = dtNow.Month;

            var descricao = UtilRequest.getString("descricao");
            var idCentroCusto = UtilRequest.getInt32("idCentroCusto");
            var idMacroConta = UtilRequest.getInt32("idMacroConta");
            var idContaBancaria = UtilRequest.getInt32("idContaBancaria");
            var flagPago = UtilRequest.getString("flagPago");
            var flagTipoSaida = UtilRequest.getString("flagTipoSaida");
            var pesquisarPor = UtilRequest.getString("pesquisarPor");
            var listaCredores = UtilRequest.getListString("listaCredores");
            DateTime? dtInicio = UtilRequest.getDateTime("dtInicio") ?? new DateTime(dtNowYear, dtNowMonth, 1);
            DateTime? dtFim = UtilRequest.getDateTime("dtFim") ?? new DateTime(dtNowYear, dtNowMonth, DateTime.DaysInMonth(dtNowYear, dtNowMonth));

            var ViewModel = new LancamentoDespesasVM();

            var query = this.OTituloDespesaPagamentoResumoVWBL.listarPagamentoDespesasExcluidas(descricao, idCentroCusto, flagPago, pesquisarPor, dtInicio, dtFim, idMacroConta, idContaBancaria);

            if (listaCredores.Any()) {
                query = query.Where(x => listaCredores.Contains(x.idCredor));
            }

            var lista = query.OrderBy(x => x.dtVencimentoDespesa).ToList();

            if (flagTipoSaida == TipoSaidaHelper.EXCEL) {

                var OLancamentoDespesaExportacao = new LancamentoDespesaExportacao();
                OLancamentoDespesaExportacao.baixarExcel(lista);

                return null;
            }

            ViewBag.dtInicio = dtInicio.Value.ToShortDateString();
            ViewBag.dtFim = dtFim.Value.ToShortDateString();

            ViewModel.listaTituloDespesaPagamento = lista.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());

            return View(ViewModel);
        }
    }
}