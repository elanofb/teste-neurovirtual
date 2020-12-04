using System.Data.Entity.ModelConfiguration;
using DAL.Entities;

namespace DAL.Planos {

	//
	public class PlanoPeriodo : DefaultEntity {

		public string descricao { get; set; }
	}

	//
	internal sealed class PlanoPeriodoMapper : EntityTypeConfiguration<PlanoPeriodo> {

		public PlanoPeriodoMapper() {
			this.ToTable("tb_plano_periodo");
			this.HasKey(o => o.id);
		}
	}
}