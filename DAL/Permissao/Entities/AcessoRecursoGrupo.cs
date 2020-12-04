using System.Data.Entity.ModelConfiguration;

namespace DAL.Permissao {

	public class AcessoRecursoGrupo {

		public int id { get; set; }

		public string descricao { get; set; }

		public string area { get; set; }

		public string controller { get; set; }

		public string action { get; set; }

		public string iconeClasse { get; set; }
		
		public byte ordem { get; set; }

		public bool? flagMenuLateral { get; set; }

        public bool? flagMenuTopo { get; set; }

        public string ativo { get; set; }

		public string flagExcluido { get; set; }

	}

	//
	internal sealed class AcessoRecursoGrupoMapper : EntityTypeConfiguration<AcessoRecursoGrupo> {

		public AcessoRecursoGrupoMapper() {
			this.ToTable("systb_acesso_recurso_grupo");
			this.HasKey(o => o.id);
		}
	}
}