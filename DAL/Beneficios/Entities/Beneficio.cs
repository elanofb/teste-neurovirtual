using System.Data.Entity.ModelConfiguration;
using DAL.Entities;

namespace DAL.Beneficios {

	//
	public class Beneficio : Geral {
	}

	//
	internal sealed class BeneficioMapper : EntityTypeConfiguration<Beneficio> {

		public BeneficioMapper() {
			this.ToTable("tb_beneficio");
			this.HasKey(o => o.id);
		}
	}
}