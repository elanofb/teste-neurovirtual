using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Contribuicoes {

	public class PeriodoContribuicao {

		public int id { get; set; }

		public string descricao { get; set; }

        public short qtdeDias { get; set; }

        public byte qtdeMeses { get; set; }

        public byte qtdeAnos { get; set; }

		public DateTime dtCadastro { get; set; }

		public DateTime dtAlteracao { get; set; }

		public int idUsuarioCadastro { get; set; }

		public int idUsuarioAlteracao { get; set; }

		public bool ativo { get; set; }

		public bool flagExcluido { get; set; }

        public bool flagSistema { get; set; }
	}

	internal sealed class PeriodoContribuicaoMapper : EntityTypeConfiguration<PeriodoContribuicao> {

		public PeriodoContribuicaoMapper() {

            this.ToTable("datatb_periodo_contribuicao");

            this.HasKey(x => x.id);
		}
	}
}