using System;
using System.Collections.Generic;
using DAL.Financeiro;
using PagedList;

namespace WEB.Areas.LancamentoRecebimentos.ViewModels {

    public class LancamentoDespesasVM {
                
        public List<string> listaCredores { get; set; }
        public List<string> listaNomeCredores { get; set; }
        public string valorBusca { get; set; }
        public int? idCentroCusto { get; set; }
        public int? idMacroConta { get; set; }
        public int? idContaBancaria { get; set; }
        public string flagPago { get; set; }
        public string flagTipoSaida { get; set; }
        public string pesquisarPor { get; set; }
        public DateTime? dtInicio { get; set; }
        public DateTime? dtFim { get; set; }
        
        public string situacaoArquivoRemessa { get; set; }

        public IPagedList<TituloDespesaPagamentoResumoVW> listaTituloDespesaPagamento { get; set; }

        public decimal totalDespesasRecebidas { get; set; }

        public decimal totalDespesasEmAberto { get; set; }

        public decimal totalDespesasAtraso { get; set; }

        public LancamentoDespesasVM() {
            this.listaTituloDespesaPagamento = new List<TituloDespesaPagamentoResumoVW>().ToPagedList(1, 20);
            this.listaCredores = new List<string>();
            this.listaNomeCredores = new List<string>();
        }

    }

}