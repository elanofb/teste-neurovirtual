using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using DAL.Pessoas;

namespace DAL.Empresas {

	//
	public class Empresa : DefaultEntity {

		public int idPessoa { get; set; }

		public virtual Pessoa Pessoa { get; set; }

		public Empresa() {

		}

	}

	//
	internal sealed class EmpresaMapper : EntityTypeConfiguration<Empresa> {

		public EmpresaMapper() {
			this.ToTable("tb_empresa");
			this.HasKey(o => o.id);

			this.Property(o => o.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			this.HasRequired(o => o.Pessoa).WithMany().HasForeignKey(o => o.idPessoa);
		}
	}
}