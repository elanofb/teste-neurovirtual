using System;
using System.Linq;
using System.Web.Mvc;
using BLL.FinanceiroLancamentos;
using WEB.Areas.LancamentoRecebimentos.ViewModels;
using PagedList;
using WEB.Helpers;
using WEB.Areas.FinanceiroLancamentos.ViewModels;

namespace WEB.Areas.FinanceiroLancamentos.Controllers {

    [OrganizacaoFilter]
    public class LancamentoDespesasController : Controller {

        //Atributos
        private ITituloDespesaPagamentoResumoVWBL _TituloDespesaPagamentoResumoVWBL;
        
        //Propriedades
        private ITituloDespesaPagamentoResumoVWBL OTituloDespesaPagamentoResumoVWBL => (this._TituloDespesaPagamentoResumoVWBL = this._TituloDespesaPagamentoResumoVWBL ?? new TituloDespesaPagamentoResumoVWBL());
        
        public ActionResult listar(LancamentoDespesasVM ViewModel) {
            ViewModel.dtInicio = ViewModel.dtInicio ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            ViewModel.dtFim = ViewModel.dtFim ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            var query = this.OTituloDespesaPagamentoResumoVWBL.listarPagamentoDespesas(ViewModel.valorBusca, ViewModel.idCentroCusto.toInt(), ViewModel.idMacroConta.toInt(), ViewModel.idContaBancaria.toInt(), ViewModel.flagPago, ViewModel.pesquisarPor, ViewModel.dtInicio, ViewModel.dtFim);
            if (ViewModel.situacaoArquivoRemessa == "G") {
                query = query.Where(x => x.idArquivoRemessa > 0);
            }
            if (ViewModel.situacaoArquivoRemessa == "NG") {
                query = query.Where(x => x.idArquivoRemessa == 0 || x.idArquivoRemessa == null);
            }
            if (ViewModel.listaCredores.Any()) {
                query = query.Where(x => ViewModel.listaCredores.Contains(x.idCredor));
            }
            if (ViewModel.flagTipoSaida == TipoSaidaHelper.EXCEL) {
                var OLancamentoDespesaExportacao = new LancamentoDespesaExportacao();
                OLancamentoDespesaExportacao.baixarExcel(query.ToList());
                return null;
            }
            var listaResumo = query.Select(x => new { x.dtPagamento , x.dtVencimentoDespesa, x.valorOriginal, x.valorTotal, x.idTituloPagamento, x.valorPago}).ToList();
            var listaDespesasRecebidas = listaResumo.Where(x => x.dtPagamento != null).ToList();
            var listaDespesasEmAberto = listaResumo.Where(x => x.dtPagamento == null).ToList();
            var listaDespesasAtraso = listaResumo.Where(x => x.dtPagamento == null && x.dtVencimentoDespesa < DateTime.Now).ToList();

            ViewModel.totalDespesasRecebidas = (listaDespesasRecebidas.Count > 0) ? listaDespesasRecebidas.Sum(x => UtilNumber.toDecimal(x.valorPago)) : 0;
            ViewModel.totalDespesasEmAberto = (listaDespesasEmAberto.Count > 0) ? listaDespesasEmAberto.Sum(x => x.valorOriginal.toDecimal()) : 0;
            ViewModel.totalDespesasAtraso = (listaDespesasAtraso.Count > 0) ? listaDespesasAtraso.Sum(x => x.valorOriginal.toDecimal()) : 0;

            ViewModel.listaNomeCredores = query.Select(x => x.nomePessoa).Distinct().OrderBy(x => x).ToList();

            ViewModel.listaTituloDespesaPagamento = query.OrderByDescending(x => x.dtVencimentoDespesa ?? x.dtVencimentoTitulo).ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());

            return View(ViewModel);
        }
    }
}