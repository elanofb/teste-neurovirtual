using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Financeiro {

	//
	public class TituloReceitaResumoVW {

		public int id { get; set; }

        public int idOrganizacao { get; set; }

        public int idReceita { get; set; }

        public byte idTipoReceita { get; set; }

		public string descricaoTipoReceita { get; set; }

		public DateTime? dtQuitacao { get; set; }

		public DateTime? dtLimitePagamento { get; set; }

		public DateTime? dtVencimentoOriginal { get; set; }

		public DateTime? dtVencimento { get; set; }

		public DateTime? dtCompetencia { get; set; }

		public string descricaoTitulo { get; set; }

		public decimal? valorTotal { get; set; }

		public decimal? valorJuros { get; set; }

		public string nomePessoa { get; set; }

		public string documentoPessoa { get; set; }

		public int qtdeParcelas { get; set; }

		public DateTime? dtExclusao { get; set; }

		public int? idUsuarioExclusao { get; set; }

		public string motivoExclusao { get; set; }
	}

	//
	internal sealed class TituloReceitaResumoVWMapper : EntityTypeConfiguration<TituloReceitaResumoVW> {

		public TituloReceitaResumoVWMapper() {

            this.ToTable("vw_titulo_receita_resumo");

            this.HasKey(o => o.id);
		}
	}
}