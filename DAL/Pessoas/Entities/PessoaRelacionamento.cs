using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using DAL.Permissao;
using DAL.Relacionamentos;

namespace DAL.Pessoas {

	[Serializable]
	public class PessoaRelacionamento  {

		public int id { get; set; }

		public int idPessoa { get; set; }

		public Pessoa Pessoa { get; set; }

		public int idOcorrenciaRelacionamento { get; set; }

		public OcorrenciaRelacionamentoVW OcorrenciaRelacionamento { get; set; }

		public DateTime? dtOcorrencia { get; set; }

		public string observacao { get; set; }

		public DateTime dtCadastro { get; set; }

		public DateTime dtAlteracao { get; set; }

		public int? idUsuarioCadastro { get; set; }

		public virtual UsuarioSistema UsuarioCadastro { get; set; }

		public int? idUsuarioAlteracao { get; set; }

		public bool? flagPossuiArquivo { get; set; }

		public string ativo { get; set; }

		public string flagExcluido { get; set; }
	}

	//
	internal sealed class PessoaRelacionamentoMapper : EntityTypeConfiguration<PessoaRelacionamento> {

		public PessoaRelacionamentoMapper() {
			
			this.ToTable("tb_pessoa_relacionamento");
			
			this.HasKey(o => o.id);
			
			this.Ignore(x => x.ativo);

			this.Ignore(x => x.flagPossuiArquivo);
			
			this.HasRequired(x => x.Pessoa).WithMany().HasForeignKey(x => x.idPessoa);

			this.HasRequired(x => x.OcorrenciaRelacionamento).WithMany().HasForeignKey(x => x.idOcorrenciaRelacionamento);
			
			this.HasOptional(x => x.UsuarioCadastro).WithMany().HasForeignKey(x => x.idUsuarioCadastro);
			
		}
	}
}