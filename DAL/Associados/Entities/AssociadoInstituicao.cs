using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Instituicoes;
using DAL.Permissao;

namespace DAL.Associados {

	//
	[Serializable]
	public class AssociadoInstituicao {

		public int id { get; set; }

		public int idAssociado { get; set; }

		public virtual Associado Associado { get; set; }

		public int idInstituicao { get; set; }

		public virtual Instituicao Instituicao { get; set; }

		public string observacao1 { get; set; }

		public string observacao2 { get; set; }

        public DateTime? dtCadastro { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public virtual UsuarioSistema UsuarioCadastro { get; set; }

		public bool? ativo { get; set; }

		public DateTime? dtExclusao { get; set; }

		public int? idUsuarioExclusao { get; set; }

        public string motivoExclusao { get; set; }
	}

	//
	internal sealed class AssociadoInstituicaoMapper : EntityTypeConfiguration<AssociadoInstituicao> {

		public AssociadoInstituicaoMapper() {

			this.ToTable("tb_associado_instituicao");

            this.HasKey(o => o.id);

			this.HasRequired(o => o.Associado).WithMany().HasForeignKey(o => o.idAssociado);

			this.HasRequired(o => o.Instituicao).WithMany().HasForeignKey(o => o.idInstituicao);

		    this.HasOptional(x => x.UsuarioCadastro).WithMany().HasForeignKey(x => x.idUsuarioCadastro);
		}
	}
}