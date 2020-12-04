using System.Data.Entity.ModelConfiguration;

namespace DAL.Contratos {

	//
	public class StatusContrato {

		public int id { get; set; }

		public string descricao { get; set; }

		public string ativo { get; set; }

		public string flagExcluido { get; set; }

		public string flagFinalizador { get; set; }
	}

	/**
	*
	*/

	internal sealed class StatusContratoMapper : EntityTypeConfiguration<StatusContrato> {

		public StatusContratoMapper() {
			this.ToTable("datatb_status_contrato");
			this.HasKey(o => o.id);
		}
	}
}