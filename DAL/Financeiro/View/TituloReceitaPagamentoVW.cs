using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Financeiro {

	//
	public class TituloReceitaPagamentoVW {
		
        public int idOrganizacao { get; set; }

        public int? idUnidade { get; set; }

        public int idTituloReceitaPagamento { get; set; }

        public int idTituloReceita { get; set; }

		public int idReceita { get; set; }

		public byte idTipoReceita { get; set; }

		public string descricaoTipoReceita { get; set; }

		public byte? mesCompetencia { get; set; }
        
		public short? anoCompetencia { get; set; }

		public string descricaoTitulo { get; set; }
        
		public string nomePessoa { get; set; }
        
		public string documentoPessoa { get; set; }
        
		public string nomeRecibo { get; set; }
        
		public string documentoRecibo { get; set; }
		
		public DateTime? dtQuitacao { get; set; }
		
		public decimal valorOriginal { get; set; }

		public decimal? valorRecebido { get; set; }

		public decimal valorDescontoCupom { get; set; }
		
		public decimal valorDescontoAntecipacao { get; set; }

		public byte qtdeParcelas { get; set; }
		
		public int? idParcelaPrincipal { get; set; }
		
		public byte? idFormaPagamento { get; set; }

		public string descricaoFormaPagamento { get; set; }

		public byte? idMeioPagamento { get; set; }

		public string descricaoMeioPagamento { get; set; }

		public byte? idGatewayPagamento { get; set; }

		public string descricaoGatewayPagamento { get; set; }

		public byte? idStatusPagamento { get; set; }

		public string descricaoStatusPagamento { get; set; }

		public DateTime? dtPagamento { get; set; }

		public DateTime? dtBaixa { get; set; }

		public DateTime? dtVencimento { get; set; }

		public DateTime? dtCredito { get; set; }

		public int? idUsuarioBaixa { get; set; }

		public string codigoAutorizacao { get; set; }

		public decimal? valorJuros { get; set; }

		public decimal? valorTarifasBancarias { get; set; }

		public decimal? valorTarifasTransacao { get; set; }

		public decimal? valorOutrasTarifas { get; set; }

        public decimal valorTotalTarifas { get; set; }

		public DateTime dtCadastro { get; set; }

        public string boletoUrl { get; set; }

        public string nomeUsuarioBaixa { get; set; }

        public string nroConta { get; set; }

        public string nroAgencia { get; set; }

        public string nossoNumero { get; set; }

        public int? idContaBancaria { get; set; }

        public int? idArquivoRemessa { get; set; }

        public int? nroArquivoRemessa { get; set; }

        public string nroAgenciaContaBancaria { get; set; }

        public string nroContaContaBancaria { get; set; }

        public int? idCupomDesconto { get; set; }

        public DateTime? dtExclusao { get; set; }

        public string motivoExclusao { get; set; }

        public string nomeUsuarioExclusao { get; set; }
    }

	//
	internal sealed class TituloReceitaPagamentoVWMapper : EntityTypeConfiguration<TituloReceitaPagamentoVW> {

		public TituloReceitaPagamentoVWMapper() {
			
            this.ToTable("vw_titulo_receita_pagamento");

			this.HasKey(o => o.idTituloReceitaPagamento);

		}
	}
}