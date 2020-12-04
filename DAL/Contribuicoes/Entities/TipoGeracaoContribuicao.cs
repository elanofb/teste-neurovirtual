using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Contribuicoes {

	public class TipoGeracaoContribuicao {

		public int id { get; set; }

		public string descricao { get; set; }

		public DateTime dtCadastro { get; set; }

		public DateTime dtAlteracao { get; set; }

		public int idUsuarioCadastro { get; set; }

		public int idUsuarioAlteracao { get; set; }

		public bool ativo { get; set; }

		public bool flagExcluido { get; set; }

		public bool flagSistema { get; set; }
    }

	internal sealed class TipoGeracaoContribuicaoMapper : EntityTypeConfiguration<TipoGeracaoContribuicao> {

		public TipoGeracaoContribuicaoMapper() {

			this.ToTable("tb_tipo_geracao_contribuicao");

            this.HasKey(x => x.id);
		}
	}
}