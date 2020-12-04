using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;
using BLL.FinanceiroLancamentos;
using WEB.Areas.FinanceiroLancamentos.ViewModels;
using WEB.Areas.LancamentoRecebimentos.ViewModels;
using WEB.Helpers;

namespace WEB.Areas.FinanceiroLancamentos.Controllers
{
    public class ReceitaExcluidaController : Controller
    {

        //Atributos
        private ITituloReceitaPagamentoResumoVWBL _TituloReceitaPagamentoResumoVWBL;

        //Propriedades
        private ITituloReceitaPagamentoResumoVWBL OTituloReceitaPagamentoResumoVWBL => (this._TituloReceitaPagamentoResumoVWBL = this._TituloReceitaPagamentoResumoVWBL ?? new TituloReceitaPagamentoResumoVWBL());

        // GET: FinanceiroLancamentos/ReceitaExcluida
        public ActionResult listar() {

            var descricao = UtilRequest.getString("descricao");
            var idCentroCusto = UtilRequest.getInt32("idCentroCusto");
            var idMacroConta = UtilRequest.getInt32("idMacroConta");
            var idContaBancaria = UtilRequest.getInt32("idContaBancaria");
            var flagPago = UtilRequest.getString("flagPago");
            var idTipoReceita = UtilRequest.getInt32("idTipoReceita");
            var flagTipoSaida = UtilRequest.getString("flagTipoSaida");
            var pesquisarPor = UtilRequest.getString("pesquisarPor");
            DateTime? dtInicio = UtilRequest.getDateTime("dtInicio") ?? DateTime.Today;
            DateTime? dtFim = UtilRequest.getDateTime("dtFim") ?? DateTime.Today;

            var ViewModel = new LancamentoRecebimentoVM();

            var lista = this.OTituloReceitaPagamentoResumoVWBL.listarPagamentoReceitasExcluidas(descricao, idCentroCusto, idTipoReceita, flagPago, pesquisarPor, dtInicio, dtFim, idMacroConta, idContaBancaria).ToList();

            if (flagTipoSaida == TipoSaidaHelper.EXCEL) {

                var OLancamentoRecebimentoExportacao = new LancamentoRecebimentoExportacao();
                OLancamentoRecebimentoExportacao.baixarExcel(lista);

                return null;
            }

            ViewBag.dtInicio = dtInicio.Value.ToShortDateString();
            ViewBag.dtFim = dtFim.Value.ToShortDateString();

            ViewModel.listaTituloReceitaPagamento = lista.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());

            return View(ViewModel);
        }
    }
}