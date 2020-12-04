using System;
using System.Collections.Generic;
using DAL.Financeiro;
using PagedList;

namespace WEB.Areas.LancamentoRecebimentos.ViewModels {

    public class LancamentoRecebimentoVM {

        public List<int> idsTipoReceita { get; set; }

        public string valorBuscaLote { get; set; }

        public string valorBusca { get; set; }

        public int? idCentroCusto { get; set; }

        public int? idMacroConta { get; set; }

        public int? idContaBancaria { get; set; }

        public byte? idGateway { get; set; }
        
        public byte? idMeioPagamento { get; set; }

        public string flagPago { get; set; }

        public string flagTipoSaida { get; set; }

        public string pesquisarPor { get; set; }

        public string flagTipoBaixa { get; set; }

        public DateTime? dtInicio { get; set; }

        public DateTime? dtFim { get; set; }

        public IPagedList<TituloReceitaPagamentoResumoVW> listaTituloReceitaPagamento { get; set; }

        public decimal? totalReceitasRecebidas { get; set; }
        
        public decimal? totalReceitasLiquidaRecebidas { get; set; }

        public decimal? totalReceitasEmAberto { get; set; }

        public decimal? totalReceitasAtraso { get; set; }

        public LancamentoRecebimentoVM() {
            this.listaTituloReceitaPagamento = new List<TituloReceitaPagamentoResumoVW>().ToPagedList(1, 20);
            this.idsTipoReceita = new List<int>();
        }
    }

}