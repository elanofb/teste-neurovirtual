using System;
using System.Data.Entity.ModelConfiguration;
using DAL.AreasAtuacao;
using DAL.Permissao;

namespace DAL.Associados {

	//
	[Serializable]
	public class AssociadoAreaAtuacao {

		public int id { get; set; }

		public int idAssociado { get; set; }

		public virtual Associado Associado { get; set; }

		public int idAreaAtuacao { get; set; }

		public virtual AreaAtuacao AreaAtuacao { get; set; }

		public string observacao1 { get; set; }

		public string observacao2 { get; set; }

		public string flagExcluido { get; set; }

		public string ativo { get; set; }

		public DateTime dtCadastro { get; set; }

		public DateTime dtAlteracao { get; set; }

		public int? idUsuarioCadastro { get; set; }

        public virtual UsuarioSistema UsuarioCadastro { get; set; }

		public int idUsuarioAlteracao { get; set; }
	}

	//
	internal sealed class AssociadoAreaAtuacaoMapper : EntityTypeConfiguration<AssociadoAreaAtuacao> {

		public AssociadoAreaAtuacaoMapper() {
			this.ToTable("tb_associado_area_atuacao");
			this.HasKey(o => o.id);

			this.HasRequired(o => o.AreaAtuacao).WithMany().HasForeignKey(o => o.idAreaAtuacao);

		    this.HasOptional(x => x.UsuarioCadastro).WithMany().HasForeignKey(x => x.idUsuarioCadastro);
		}
	}
}