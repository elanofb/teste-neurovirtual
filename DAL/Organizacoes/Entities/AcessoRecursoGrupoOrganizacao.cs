using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Permissao;

namespace DAL.Organizacoes {

	public class AcessoRecursoGrupoOrganizacao {

		public int id { get; set; }

	    public int idOrganizacao { get; set; }

	    public virtual Organizacao Organizacao { get; set; }

	    public int idRecursoGrupo { get; set; }

	    public virtual AcessoRecursoGrupo AcessoRecursoGrupo { get; set; }

        public DateTime dtAtivacao { get; set; }

        public int idUsuarioAtivacao { get; set; }

	    public virtual UsuarioSistema UsuarioAtivacao { get; set; }
        
	    public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }

	    public virtual UsuarioSistema UsuarioExclusao { get; set; }
        
    }

	//
	internal sealed class AcessoRecursoGrupoOrganizacaoMapper : EntityTypeConfiguration<AcessoRecursoGrupoOrganizacao> {

		public AcessoRecursoGrupoOrganizacaoMapper() {

            //
			this.ToTable("systb_acesso_recurso_grupo_organizacao");

			this.HasKey(o => o.id);

            //  FKs
		    this.HasRequired(u => u.Organizacao).WithMany().HasForeignKey(u => u.idOrganizacao);

		    this.HasRequired(u => u.AcessoRecursoGrupo).WithMany().HasForeignKey(u => u.idRecursoGrupo);

            this.HasRequired(u => u.UsuarioAtivacao).WithMany().HasForeignKey(u => u.idUsuarioAtivacao);

		    this.HasOptional(u => u.UsuarioExclusao).WithMany().HasForeignKey(u => u.idUsuarioExclusao);

        }
	}
}