using System.Data.Entity.ModelConfiguration;
using DAL.Entities;

namespace DAL.Contribuicoes {

	public class TipoContribuicao : Geral {

		public string flagSistema { get; set; }
	}

	internal sealed class TipoContribuicaoMapper : EntityTypeConfiguration<TipoContribuicao> {

		public TipoContribuicaoMapper() {
			this.ToTable("tb_tipo_contribuicao");
			this.HasKey(x => x.id);
		}
	}
}