using System.Data.Entity.ModelConfiguration;
using DAL.Entities;

namespace DAL.Escolaridades {

	//
	public class NivelEscolar : Geral {

		public string flagSistema { get; set; }
	}

	//
	internal sealed class NivelEscolarMapper : EntityTypeConfiguration<NivelEscolar> {

		public NivelEscolarMapper() {
			this.ToTable("tb_nivel_escolar");
			this.HasKey(o => o.id);
		}
	}
}