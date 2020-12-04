using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Cargos;
using DAL.Entities;

namespace DAL.Associados {

	//
	[Serializable]
	public class AssociadoCargo : DefaultEntity {

		public int idAssociado { get; set; }

		public virtual Associado Associado { get; set; }

		public int idCargo { get; set; }

		public virtual Cargo Cargo { get; set; }

		public string inicioGestao { get; set; }

		public string fimGestao { get; set; }
	}

	//
	internal sealed class AssociadoCargoMapper : EntityTypeConfiguration<AssociadoCargo> {

		public AssociadoCargoMapper() {
			this.ToTable("tb_associado_cargo");
			this.HasKey(o => o.id);

			this.HasRequired(o => o.Cargo).WithMany().HasForeignKey(o => o.idCargo);
			this.HasRequired(o => o.Associado).WithMany().HasForeignKey(o => o.idAssociado);
		}
	}
}