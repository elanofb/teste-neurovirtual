using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using DAL.Emails;

namespace DAL.Pessoas {

	//
	[Serializable]
	public class PessoaEmail  {

		public int id { get; set; }

        public int? idOrganizacao { get; set; }

		public int idPessoa { get; set; }

		public virtual Pessoa Pessoa { get; set; }

		public byte? idTipoEmail { get; set; }

		public virtual TipoEmail TipoEmail { get; set; }

		[MaxLength(100)]
		public string email { get; set; }

		public DateTime? dtCadastro { get; set; }

		public int? idUsuarioCadastro { get; set; }

		public DateTime? dtAlteracao { get; set; }

		public int? idUsuarioAlteracao { get; set; }

		public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }

        public string motivoExclusao { get; set; }
	}

	//
	internal sealed class PessoaEmailMapper : EntityTypeConfiguration<PessoaEmail> {

		public PessoaEmailMapper() {

			this.ToTable("tb_pessoa_email");

            this.HasKey(o => o.id);

			this.HasRequired(o => o.Pessoa).WithMany(p => p.listaEmails).HasForeignKey(o => o.idPessoa);

			this.HasOptional(o => o.TipoEmail).WithMany().HasForeignKey(o => o.idTipoEmail);
		}
	}
}