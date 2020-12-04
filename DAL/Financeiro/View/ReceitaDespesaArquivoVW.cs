using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Financeiro {

	//
	public class ReceitaDespesaArquivoVW {
		
		public int id { get; set; }
		
		public int? idOrganizacao { get; set; }
		
		public DateTime dtCadastro { get; set; }
		
		public string titulo { get; set; }
		
		public string legenda { get; set; }
		
		public string path { get; set; }
		
		public int idPagamento { get; set; }
		
		public int idTitulo { get; set; }
		
		public string flagTipoTitulo { get; set; }
		
		public byte? idTipoTitulo { get; set; }
		
		public string descricaoTipoTitulo { get; set; }
		
		public string descricaoTitulo { get; set; }
		
		public string descricaoParcela { get; set; }
		
		public string nomePessoa { get; set; }
		
		public DateTime? dtVencimento { get; set; }
		
		public DateTime? dtPagamento { get; set; }
		
		public decimal valor { get; set; }
		
		public decimal valorDesconto { get; set; }
		
		public decimal valorDescontoCupom { get; set; }
		
		public decimal valorDescontoAntecipacao { get; set; }
		
		public int? qtdeParcelas { get; set; }
		
		public decimal valorTotalTitulo { get; set; }
		
	}

	//
	internal sealed class ReceitaDespesaArquivoVWMapper : EntityTypeConfiguration<ReceitaDespesaArquivoVW> {

		public ReceitaDespesaArquivoVWMapper() {
			
            this.ToTable("vw_receitas_despesas_arquivo");

			this.HasKey(o => o.id);

		}

	}

}