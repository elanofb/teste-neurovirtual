using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using DAL.Permissao;

namespace DAL.Organizacoes {

	//
	public class UsuarioOrganizacao : DefaultEntity {

		public int idUsuario { get; set; }

		public virtual UsuarioSistema UsuarioSistema { get; set; }

		public int idOrganizacao { get; set; }

		public virtual Organizacao Organizacao { get; set; }
	}

	//
	internal sealed class UsuarioOrganizacaoMapper : EntityTypeConfiguration<UsuarioOrganizacao> {

		public UsuarioOrganizacaoMapper() {
			this.ToTable("tb_usuario_organizacao");
			this.HasKey(o => o.id);

			//FKs
			this.HasRequired(u => u.UsuarioSistema).WithMany(x => x.listaUsuarioOrganizacao).HasForeignKey(u => u.idUsuario);
			this.HasRequired(u => u.Organizacao).WithMany().HasForeignKey(u => u.idOrganizacao);
		}
	}
}