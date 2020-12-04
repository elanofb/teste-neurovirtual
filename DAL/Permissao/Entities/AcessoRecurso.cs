using System.Data.Entity.ModelConfiguration;
using DAL.Entities;

namespace DAL.Permissao {

	//
	public class AcessoRecurso : Geral {

		public int? idRecursoPai { get; set; }

		public virtual AcessoRecurso AcessoRecursoPai { get; set; }

		public int? idRecursoGrupo { get; set; }

		public virtual AcessoRecursoGrupo AcessoRecursoGrupo { get; set; }

		public string nomeDisplay { get; set; }

		public string area { get; set; }

		public string controller { get; set; }

		public string actionPadrao { get; set; }

		public int ordemExibicao { get; set; }

		public string flagAcessoLiberado { get; set; }

		public bool? flagMenu { get; set; }
	}

	//
	internal sealed class AcessoRecursoMapper : EntityTypeConfiguration<AcessoRecurso> {

		public AcessoRecursoMapper() {
			this.ToTable("systb_acesso_recurso");
			this.HasKey(o => o.id);

			this.HasOptional(x => x.AcessoRecursoGrupo).WithMany().HasForeignKey(x => x.idRecursoGrupo);
			this.HasOptional(x => x.AcessoRecursoPai).WithMany().HasForeignKey(x => x.idRecursoPai);
		}
	}
}