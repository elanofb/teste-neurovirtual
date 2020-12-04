using System.Data.Entity.ModelConfiguration;
using DAL.Permissao;
using DAL.Unidades;

namespace DAL.Entities {

	//
	public class UsuarioUnidade : DefaultEntity {

		public int idUsuario { get; set; }

		public virtual UsuarioSistema UsuarioSistema { get; set; }

		public int idUnidade { get; set; }

		public virtual Unidade Unidade { get; set; }
	}

	//
	internal sealed class UsuarioUnidadeMapper : EntityTypeConfiguration<UsuarioUnidade> {

		public UsuarioUnidadeMapper() {
			this.ToTable("tb_usuario_unidade");
			this.HasKey(o => o.id);

			//FKs
			this.HasRequired(u => u.UsuarioSistema).WithMany(x => x.listaUsuarioUnidade).HasForeignKey(u => u.idUsuario);
			this.HasRequired(u => u.Unidade).WithMany().HasForeignKey(u => u.idUnidade);
		}
	}
}