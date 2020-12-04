using System.Data.Entity.ModelConfiguration;

namespace DAL.Planos {

	//
	public class StatusPlanoContratacao {

		public int id { get; set; }

		public string descricao { get; set; }

		public string flagFinalizador { get; set; }

		public string flagExcluido { get; set; }
	}

	//
	internal sealed class StatusPlanoContratacaoMapper : EntityTypeConfiguration<StatusPlanoContratacao> {

		public StatusPlanoContratacaoMapper() {
			this.ToTable("tb_status_plano_contratacao");
			this.HasKey(o => o.id);
		}
	}
}