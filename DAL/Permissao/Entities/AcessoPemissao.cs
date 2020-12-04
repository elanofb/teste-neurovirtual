using System.Data.Entity.ModelConfiguration;
using DAL.Entities;

namespace DAL.Permissao {

	//
	public partial class AcessoPermissao : DefaultEntity {

		public int? idPerfilAcesso { get; set; }

		public virtual PerfilAcesso PerfilAcesso { get; set; }

		public int? idGrupo { get; set; }

		public virtual AcessoRecursoGrupo AcessoRecursoGrupo { get; set; }

		public int? idRecurso { get; set; }

		public virtual AcessoRecurso AcessoRecurso { get; set; }

		public int? idRecursoAcao { get; set; }

		public virtual AcessoRecursoAcao AcessoRecursoAcao { get; set; }
	}

	//
	internal sealed class AcessoPermissaoMapper : EntityTypeConfiguration<AcessoPermissao> {

		public AcessoPermissaoMapper() {
			this.ToTable("systb_acesso_permissao");
			this.HasKey(o => o.id);

			this.HasRequired(u => u.PerfilAcesso).WithMany().HasForeignKey(o => o.idPerfilAcesso);
			this.HasOptional(u => u.AcessoRecursoGrupo).WithMany().HasForeignKey(o => o.idGrupo);
			this.HasOptional(u => u.AcessoRecurso).WithMany().HasForeignKey(o => o.idRecurso);
			this.HasOptional(u => u.AcessoRecursoAcao).WithMany().HasForeignKey(o => o.idRecursoAcao);
		}
	}
}