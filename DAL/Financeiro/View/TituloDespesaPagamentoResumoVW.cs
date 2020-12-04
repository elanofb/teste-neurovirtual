using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Financeiro {

	//
	public class TituloDespesaPagamentoResumoVW {
		
        public Guid id { get; set; }

        public int idOrganizacao { get; set; }

        public int? idUnidade { get; set; }

        public string siglaUnidade { get; set; }

        public int idTituloDespesa { get; set; }

        public string descricao { get; set; }

        public int? idTipoDespesa { get; set; }

        public string flagCategoriaPessoa { get; set; }

        public int? idPessoa { get; set; }

        public DateTime? dtVencimentoTitulo { get; set; }
        public DateTime? dtQuitacao { get; set; }
        public DateTime? dtDespesa { get; set; }
        public decimal? valorTotal { get; set; }
        public string flagFixa { get; set; }
        public int? nroNotaFiscal { get; set; }
        public string observacao { get; set; }
        public int? nroContabil { get; set; }
        public int? qtdeRepeticao { get; set; }
        public int? idContaBancaria { get; set; }
	    public string idCredor { get; set; }

        public int? idTituloPagamento { get; set; }
	    public string descParcela { get; set; }
        public DateTime? dtVencimentoDespesa { get; set; }
	    public DateTime? dtDebito { get; set; }
	    public DateTime? dtPagamento { get; set; }
	    public DateTime? dtCompetencia { get; set; }
	    public byte? idStatusPagamento { get; set; }
	    public decimal? valorOriginal { get; set; }
	    public decimal? valorPago { get; set; }
	    public decimal? valorTarifasBancarias { get; set; }
	    public decimal? valorOutrasTarifas { get; set; }
	    public int? idCentroCusto { get; set; }
	    public int? idMacroConta { get; set; }
	    public int? idCategoria { get; set; }

	    public string descricaoCentroCusto { get; set; }
	    public string descricaoMacroConta { get; set; }
	    public string descricaoCategoriaPai { get; set; }
	    public string descricaoCategoria { get; set; }

	    public int? idUsuarioExclusao { get; set; }
        public DateTime? dtExclusao { get; set; }
	    public string motivoExclusao { get; set; }
        public string nomeUsuarioExclusao { get; set; }
        
        public string nomePessoa { get; set; }
        public string nroDocumentoPessoa { get; set; }
        public int? idTipoDocumentoPessoa { get; set; }

        public string descricaoStatusPagamento { get; set; }
        public string descricaoContaBancaria { get; set; }       
        public string descricaoPeriodoRepeticao { get; set; }    
		
        public string formaPagamento { get; set; }    
        public string meioPagamento { get; set; }    
		
		public int? idArquivoRemessa { get; set; }
    }

	//
	internal sealed class TituloDespesaPagamentoResumoVWMapper : EntityTypeConfiguration<TituloDespesaPagamentoResumoVW> {

		public TituloDespesaPagamentoResumoVWMapper() {
			
            this.ToTable("vw_titulo_despesa_pagamento_resumo");

			this.HasKey(o => o.id);
		}
	}
}