using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using BLL.Services;
using PagedList;
using BLL.Transacoes.Movimentos;
using DAL.Transacoes;
using WEB.Areas.Movimentacoes.ViewModels;
using WEB.Helpers;

namespace WEB.Areas.Movimentacoes.Controllers {

    public class MovimentoConsultaController : Controller {

        // Atributos
        private IExtratoConsultaBL _ExtratoConsultaBL { get; set; }

        // Propriedades
        private IExtratoConsultaBL OExtratoConsultaBL => this._ExtratoConsultaBL = this._ExtratoConsultaBL ?? new ExtratoConsultaBL();

        //Listagem para consulta de Relacionamentos existentes
        public ActionResult index() {
            
            string valorBusca = UtilRequest.getString("valorBusca");
            
            DateTime? dtInicio = UtilRequest.getDateTime("dtInicio");
            
            TimeSpan? hrInicio = UtilRequest.getTime("hrInicio");;
            
            DateTime? dtFim = UtilRequest.getDateTime("dtFim");
            
            TimeSpan? hrFim = UtilRequest.getTime("hrFim");;
            
            byte idTipoTransacao = UtilRequest.getByte("idTipoTransacao");
            
            string flagTipoSaida = UtilRequest.getString("flagTipoSaida");

            if (!dtInicio.HasValue) {

                dtInicio = DateTime.Today.AddDays(-7);
            }
            
            if (!dtFim.HasValue) {

                dtFim = DateTime.Today;
            }

            if (hrInicio.HasValue) {

                dtInicio = dtInicio.Value.AddTicks(hrInicio.Value.Ticks);
            }

            if (hrFim.HasValue) {

                dtFim = dtFim.Value.AddTicks(hrFim.Value.Ticks);
            }

            var query = this.OExtratoConsultaBL.query(0, dtInicio, dtFim);

            if (idTipoTransacao > 0) {

                query = query.Where(x => x.idTipoTransacao == idTipoTransacao);
            }
            
            if (!valorBusca.isEmpty()) {

                int buscaNumerica = valorBusca.onlyNumber().toInt();

                query = query.Where(x => x.nomeMembroDestino.Contains(valorBusca) || x.nroMembroDestino == buscaNumerica || x.idMovimento == buscaNumerica || x.idMovimentoPrincipal == buscaNumerica);
            }
            
            if (flagTipoSaida == TipoSaidaHelper.EXCEL){
            
                GeradorCsvMovimentacao OGeradorExcel = new GeradorCsvMovimentacao();
                OGeradorExcel.baixarExcel(query.ToListJsonObject<MovimentoResumoVW>());
                
                return null;

            }
            
            var listaOcorrencias = query.OrderByDescending(x => x.idMovimento)
                                       .ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());

            return View(listaOcorrencias);
        }

        //Listagem para consulta de Relacionamentos existentes
        [ActionName("partial-lista-movimentacao")]
        public ActionResult partialListaMovimentacao() {
            
            string valorBusca = UtilRequest.getString("valorBusca");
            
            DateTime? dtInicio = UtilRequest.getDateTime("dtInicio");
            
            DateTime? dtFim = UtilRequest.getDateTime("dtFim");
            
            int idMembroDestino = UtilRequest.getInt32("idMembroDestino");
            
            string flagTipoSaida = UtilRequest.getString("flagTipoSaida");
            
            if (!dtInicio.HasValue) {

                dtInicio = DateTime.Today.AddDays(-7);
            }
            
            if (!dtFim.HasValue) {

                dtFim = DateTime.Today.AddDays(1);
            }
            
            var query = this.OExtratoConsultaBL.query(idMembroDestino, dtInicio, dtFim);
            
            if (!valorBusca.isEmpty()) {

                int buscaNumerica = valorBusca.onlyNumber().toInt();

                query = query.Where(x => x.nomeMembroDestino.Contains(valorBusca) || x.nroMembroDestino == buscaNumerica || x.idMovimento == buscaNumerica || x.idMovimentoPrincipal == buscaNumerica);
            }

            

            var listaOcorrencias = query.OrderByDescending(x => x.idMovimento)
                                       .ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());

            return View(listaOcorrencias);
        }


    }
}