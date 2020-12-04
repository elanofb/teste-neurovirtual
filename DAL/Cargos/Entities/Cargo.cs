using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using DAL.Organizacoes;

namespace DAL.Cargos {

	//
	public class Cargo : Geral {

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }
    
	}

	//
	internal sealed class CargoMapper : EntityTypeConfiguration<Cargo> {

		public CargoMapper() {

			this.ToTable("tb_cargo");

			this.HasKey(o => o.id);

            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

		}
	}
}