using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Financeiro {

	//
	public class ReceitaDespesaVW {
		
		public Guid id { get; set; }
		
		public int idPagamento { get; set; }
		
		public int idTitulo { get; set; }
		
		public int? idOrganizacao { get; set; }
		
		public string flagTipoTitulo { get; set; }
		
		public int? idTipoTitulo { get; set; }
		
		public string descricaoTitulo { get; set; }
		
		public int? qtdeRepeticao { get; set; }
		
		public int? nroParcela { get; set; }
		
		public string descricaoParcela { get; set; }
		
		public string nomePessoa { get; set; }
		
		public string descricaoTipoTitulo { get; set; }
		
		public int? idCentroCusto { get; set; }
		
		public string codigoCentroCusto { get; set; }
		
		public string descricaoCentroCusto { get; set; }
		
		public int? idMacroConta { get; set; }
		
		public string codigoMacroConta { get; set; }
		
		public string descricaoMacroConta { get; set; }
		
		public int? idSubConta { get; set; }
		
		public string codigoSubConta { get; set; }
		
		public string descricaoSubConta { get; set; }
		
		public int? idSubContaPai { get; set; }
		
		public string codigoSubContaPai { get; set; }
		
		public string descricaoSubContaPai { get; set; }
		
		public int? idContaBancaria { get; set; }
		
		public string descricaoContaBancaria { get; set; }
		
		public DateTime? dtPagamento { get; set; }
		
		public DateTime? dtEfetivacao { get; set; }
		
		public DateTime? dtPrevisaoEfetivacao { get; set; }
		
		public DateTime? dtCompetencia { get; set; }
		
		public DateTime? dtCadastro { get; set; }
		
		public DateTime? dtVencimento { get; set; }
		
		public decimal valor { get; set; }
		
		public decimal? valorRealizado { get; set; }
		
		public decimal? valorJuros { get; set; }
		
		public decimal? valorDesconto { get; set; }
		
		public decimal? valorDescontoCupom { get; set; }
		
		public decimal? valorDescontoAntecipacao { get; set; }
		
		public decimal? valorTarifasBancarias { get; set; }
		
		public decimal? valorTarifasTransacao { get; set; }
		
		public decimal? valorOutrasTarifas { get; set; }

		public int? idConciliacao { get; set; }
		
		public DateTime? dtConciliacao { get; set; }
		
		public string nomeUsuarioConciliacao { get; set; }
		
		public string meioPagamento { get; set; }
		
		public string formaPagamento { get; set; }
		
		public string gatewayPagamento { get; set; }
		
		public string tokenTransacao { get; set; }
		
		// Ignore
		public DateTime? dtMovimento { get; set; }

	}

	//
	internal sealed class ReceitaDespesaVWMapper : EntityTypeConfiguration<ReceitaDespesaVW> {

		public ReceitaDespesaVWMapper() {
			
            this.ToTable("vw_receitas_despesas");

			this.HasKey(o => o.id);

			this.Ignore(o => o.dtMovimento);

		}

	}

}