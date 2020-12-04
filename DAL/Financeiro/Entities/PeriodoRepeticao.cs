using System.Data.Entity.ModelConfiguration;
using DAL.Entities;

namespace DAL.Financeiro {
	/**
	*
	*/

	public class PeriodoRepeticao : Geral {
	}

	/**
	*
	*/

	internal sealed class PeriodoRepeticaoMapper : EntityTypeConfiguration<PeriodoRepeticao> {

		public PeriodoRepeticaoMapper() {
			this.ToTable("datatb_periodo_repeticao");
			this.HasKey(o => o.id);
		}
	}
}