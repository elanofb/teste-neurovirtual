using System;
using System.Linq;
using System.Web.Mvc;
using BLL.FinanceiroLancamentos;
using BLL.Services;
using DAL.Financeiro;
using WEB.Areas.LancamentoRecebimentos.ViewModels;
using PagedList;
using WEB.Helpers;
using WEB.Areas.FinanceiroLancamentos.ViewModels;

namespace WEB.Areas.FinanceiroLancamentos.Controllers {

    [OrganizacaoFilter]
    public class LancamentoRecebimentoController : Controller {

        //Atributos
        private ITituloReceitaPagamentoResumoVWBL _TituloReceitaPagamentoResumoVWBL;
        //Propriedades
        private ITituloReceitaPagamentoResumoVWBL OTituloReceitaPagamentoResumoVWBL => (this._TituloReceitaPagamentoResumoVWBL = this._TituloReceitaPagamentoResumoVWBL ?? new TituloReceitaPagamentoResumoVWBL());

        /// <summary>
        /// Listagem de receitas de acordo com a busca realizada
        /// </summary>
        public ActionResult listar(LancamentoRecebimentoVM ViewModel){

            ViewModel.dtInicio = ViewModel.dtInicio ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            ViewModel.dtFim = ViewModel.dtFim ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            var query = this.OTituloReceitaPagamentoResumoVWBL.listarPagamentoReceitas(ViewModel.valorBusca, ViewModel.idCentroCusto.toInt(), ViewModel.idMacroConta.toInt(), ViewModel.idContaBancaria.toInt(), 0, ViewModel.flagPago, ViewModel.pesquisarPor, ViewModel.dtInicio, ViewModel.dtFim);

            if (ViewModel.idsTipoReceita.Any()) {
                var ids = ViewModel.idsTipoReceita.Select(x => x.toByte()).ToList();
                query = query.Where(x => ids.Contains(x.idTipoReceita.Value));
            }
            if (ViewModel.idGateway > 0) {
                query = query.Where(x => x.idGatewayPagamento == ViewModel.idGateway);
            }
            if (ViewModel.idMeioPagamento > 0) {
                query = query.Where(x => x.idMeioPagamento == ViewModel.idMeioPagamento);
            }
            if (ViewModel.flagTipoBaixa == "M") {
                query = query.Where(x => x.flagBaixaAutomatica != true);
            }
            if (ViewModel.flagTipoBaixa == "A") {
                query = query.Where(x => x.flagBaixaAutomatica == true);
            }
            if (!ViewModel.valorBuscaLote.isEmpty()) {
                string[] separadores = { "\r\n" };
                string[] valoresBusca = ViewModel.valorBuscaLote.Split(separadores, StringSplitOptions.None).Where(x => !x.isEmpty()).ToArray();
                var valoresNumericos = valoresBusca.Select(x => (int?) x.toInt()).Where(x => x > 0).ToList();
                var valoresSoNumeros = valoresBusca.Select(UtilString.onlyNumber).Where(x => !x.isEmpty()).ToList();
                query = query.Where(x => valoresNumericos.Contains(x.idTituloPagamento) ||
                                         valoresNumericos.Contains(x.idTituloReceita) ||
                                         valoresSoNumeros.Contains(x.tokenTransacao) ||
                                         valoresSoNumeros.Contains(x.nroDocumentoPessoa));
            }
            if (ViewModel.flagTipoSaida == TipoSaidaHelper.EXCEL) {
                var OLancamentoRecebimentoExportacao = new LancamentoRecebimentoExportacao();
                OLancamentoRecebimentoExportacao.baixarExcel(query.ToList());
                return null;
            }
            var listaResumo = query.Select(
                x => new {
                    x.dtPagamento,
                    x.dtPrevisaoCredito,
                    x.dtCredito,
                    x.idArquivoRemessa,
                    x.valorRecebido,
                    x.valorOriginal,
                    x.valorJuros,
                    x.valorDesconto,
                    x.valorDescontoAntecipacao,
                    x.valorDescontoCupom,
                    x.valorTarifasBancarias,
                    x.valorTarifasTransacao,
                    x.valorOutrasTarifas,
                    x.dtVencimentoRecebimento
                })
                .ToListJsonObject<TituloReceitaPagamentoResumoVW>();

            var listaReceitasRecebidas = listaResumo.Where(x => x.dtPagamento != null).ToList();
            var listaReceitasEmAberto = listaResumo.Where(x => x.dtPagamento == null).ToList();
            var listaReceitasAtraso = listaResumo.Where(x => x.dtPagamento == null && x.dtVencimentoRecebimento < DateTime.Now).ToList();

            ViewModel.totalReceitasRecebidas = (listaReceitasRecebidas.Count > 0) ? listaReceitasRecebidas.Sum(x => x.valorRecebido) : 0;
            ViewModel.totalReceitasLiquidaRecebidas = (listaReceitasRecebidas.Count > 0) ? listaReceitasRecebidas.Sum(x => x.valorLiquido()) : 0;
            ViewModel.totalReceitasEmAberto = (listaReceitasEmAberto.Count > 0) ? listaReceitasEmAberto.Sum(x => x.valorComJurosEDescontos()) : 0;
            ViewModel.totalReceitasAtraso = (listaReceitasAtraso.Count > 0) ? listaReceitasAtraso.Sum(x => x.valorComJurosEDescontos()) : 0;

            query = query.OrderBy(x => x.dtVencimentoRecebimento);

            if       (ViewModel.pesquisarPor == "P") {
                query = query.OrderBy(x => x.dtPagamento);
            }else if(ViewModel.pesquisarPor == "C") {
                query = query.OrderBy(x => x.dtCredito);
            }else if(ViewModel.pesquisarPor == "PC") {
                query = query.OrderBy(x => x.dtPrevisaoCredito);
            } 
            
            ViewModel.listaTituloReceitaPagamento = query.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());

            return View(ViewModel);
        }
    }
}