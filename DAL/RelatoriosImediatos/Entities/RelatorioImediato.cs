using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.RelatoriosImediatos {

	public class RelatorioImediato {

        public int id { get; set; }

        public int idOrganizacao { get; set; }

		public string tituloRelatorio { get; set; }

		public string descricao { get; set; }

		public int? idRecurso { get; set; }

		public string nomeProcedure { get; set; }

		public DateTime dtCadastro { get; set; }

        public int idUsuarioCadastro { get; set; }

		public DateTime dtAlteracao { get; set; }

        public int idUsuarioAlteracao { get; set; }

		public string ativo { get; set; }

		public string flagExcluido { get; set; }

		public string flagSistema { get; set; }
	}


	internal sealed class RelatorioImediatoMapper : EntityTypeConfiguration<RelatorioImediato> {

		public RelatorioImediatoMapper() {

			this.ToTable("systb_relatorio_imediato");

            this.HasKey(o => o.id);
		}
	}
}