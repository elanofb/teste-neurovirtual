using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.AssociadosContribuicoes {

	//
	public class AssociadoContribuicaoEmailCobranca {

		public int id { get; set; }

		public int idAssociadoContribuicao { get; set; }

		public virtual AssociadoContribuicao AssociadoContribuicao { get; set; }

		public string nomeAssociado { get; set; }

		public string emailPrincipal { get; set; }

		public string emailSecundario { get; set; }

		public int idTarefa { get; set; }

		public bool flagEnvio { get; set; }

		public DateTime? dtEnvio { get; set; }

		public bool flagExcluido { get; set; }

	}

	//
	internal sealed class AssociadoContribuicaoEmailCobrancaMapper : EntityTypeConfiguration<AssociadoContribuicaoEmailCobranca> {

		public AssociadoContribuicaoEmailCobrancaMapper() {
			this.ToTable("tb_associado_contribuicao_email_cobranca");
			this.HasKey(o => o.id);

			this.HasRequired(o => o.AssociadoContribuicao).WithMany().HasForeignKey(o => o.idAssociadoContribuicao);
		}
	}
}