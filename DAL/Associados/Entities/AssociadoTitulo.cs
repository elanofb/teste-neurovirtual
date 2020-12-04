using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Instituicoes;

namespace DAL.Associados {

	[Serializable]
	public class AssociadoTitulo {

		public int id { get; set; }

		public int idAssociado { get; set; }

		public virtual Associado Associado { get; set; }

		public int idTipoTitulo { get; set; }

		public virtual TipoTitulo TipoTitulo { get; set; }

		public int idInstituicao { get; set; }

		public virtual Instituicao Instituicao { get; set; }

		public DateTime dtAquisicao { get; set; }

		public DateTime? dtProximaRenovacao { get; set; }

		public int idUsuarioCadastro { get; set; }

		public int idUsuarioAlteracao { get; set; }

		public DateTime dtCadastro { get; set; }

		public DateTime dtAlteracao { get; set; }

		public string flagExcluido { get; set; }

		public string ativo { get; set; }
	}

	/**
	*
	*/

	internal sealed class AssociadoTituloMapper : EntityTypeConfiguration<AssociadoTitulo> {

		public AssociadoTituloMapper() {
			this.ToTable("tb_associado_titulo");
			this.HasKey(o => o.id);

			this.HasRequired(o => o.Associado).WithMany().HasForeignKey(o => o.idAssociado);
			this.HasRequired(o => o.TipoTitulo).WithMany().HasForeignKey(o => o.idTipoTitulo);
			this.HasRequired(o => o.Instituicao).WithMany().HasForeignKey(o => o.idInstituicao);
		}
	}
}