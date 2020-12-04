using System;

namespace WEB.Areas.FinanceiroLancamentos.ViewModels {
    
    public class TituloPagamentoResumoDTO {
        
        public int? id { get; set; }
        
        public int idTitulo { get; set; }

        public string flagTipoTitulo { get; set; }

        public string descricao { get; set; }
        
        public string descParcela { get; set; }
        
        public string descricaoCentroCusto { get; set; }
        
        public int? idUnidade { get; set; }

        public string siglaUnidade { get; set; }
        
        public DateTime? dtVencimento { get; set; }
        
        public DateTime? dtPagamento { get; set; }

        public DateTime? dtPrevisaoCredito { get; set; }
        
        public DateTime? dtCredito { get; set; }
        
        public decimal? valorOriginal { get; set; }
        
        public decimal? valorRealizado { get; set; }
        
        public decimal? valorLiquido { get; set; }
        
        public byte? idStatusPagamento { get; set; }
        
        public string descricaoStatusPagamento { get; set; }
        
        public int? idPessoa { get; set; }
        
        public string nomePessoa { get; set; }

        public int? idArquivoRemessa { get; set; }

    }
    
}