using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Localizacao;

namespace DAL.Associados {

	//
	[Serializable]
	public class AssociadoAbrangencia {

		public int id { get; set; }

		public int idAssociado { get; set; }

		public virtual Associado Associado { get; set; }

		public int idCidade { get; set; }

		public virtual Cidade Cidade { get; set; }

		public string flagExcluido { get; set; }

		public string ativo { get; set; }

        public DateTime dtCadastro { get; set; }

		public int? idUsuarioCadastro { get; set; }

		public DateTime dtAlteracao { get; set; }

		public int? idUsuarioAlteracao { get; set; }

	}

	//
	internal sealed class AssociadoAbrangenciaMapper : EntityTypeConfiguration<AssociadoAbrangencia> {

		public AssociadoAbrangenciaMapper() {
			this.ToTable("tb_associado_abrangencia");
			this.HasKey(o => o.id);

			this.HasRequired(o => o.Cidade).WithMany().HasForeignKey(o => o.idCidade);
			this.HasRequired(o => o.Associado).WithMany().HasForeignKey(o => o.idAssociado);
		}
	}
}