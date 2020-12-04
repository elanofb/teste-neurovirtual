using System.Data.Entity.ModelConfiguration;
using DAL.Entities;

namespace DAL.Relacionamentos {

	//
	public class OcorrenciaRelacionamentoPadrao : Geral {

		public string flagSistema { get; set; }
	}

	//
	internal sealed class OcorrenciaRelacionamentoPadraoMapper : EntityTypeConfiguration<OcorrenciaRelacionamentoPadrao> {

		public OcorrenciaRelacionamentoPadraoMapper() {
			
			this.ToTable("datatb_ocorrencia_relacionamento");
			
			this.HasKey(o => o.id);
			
		}
	}
}