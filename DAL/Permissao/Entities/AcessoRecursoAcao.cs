using System.Data.Entity.ModelConfiguration;
using DAL.Entities;

namespace DAL.Permissao {

	//
	public class AcessoRecursoAcao : Geral {


		public int? idRecursoGrupo { get; set; }

		public virtual AcessoRecursoGrupo RecursoGrupo { get; set; }

		public int? idRecursoPai { get; set; }

		public virtual AcessoRecurso RecursoPai { get; set; }

		public string area { get; set; }

		public string controller { get; set; }

		public string action { get; set; }

		public string method { get; set; }


	}

	//
	internal sealed class AcessoRecursoAcaoMapper : EntityTypeConfiguration<AcessoRecursoAcao> {

		public AcessoRecursoAcaoMapper() {
			this.ToTable("systb_acesso_recurso_acao");
			this.HasKey(o => o.id);

			this.HasOptional(u => u.RecursoPai).WithMany().HasForeignKey(o => o.idRecursoPai);
			this.HasOptional(u => u.RecursoGrupo).WithMany().HasForeignKey(o => o.idRecursoGrupo);
		}
	}
}