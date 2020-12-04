using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using DAL.Telefones;

namespace DAL.Pessoas {

	//
	[Serializable]
	public class PessoaTelefone  {

		public int id { get; set; }

		public int? idOrganizacao { get; set; }

		public int idPessoa { get; set; }

		public virtual Pessoa Pessoa { get; set; }

		public byte? idTipoTelefone { get; set; }

		public virtual TipoTelefone TipoTelefone { get; set; }

		public int? ddi { get; set; }

		[MaxLength(15)]
		public string nroTelefone { get; set; }

		[MaxLength(1)]
		public string flagTipo { get; set; }

        public int? idOperadoraTelefonia { get; set; }

        public virtual OperadoraTelefonia OperadoraTelefonia {get; set; }

		public bool? ativo { get; set; }

		public DateTime? dtCadastro { get; set; }

		public int? idUsuarioCadastro { get; set; }

		public DateTime? dtAlteracao { get; set; }

		public int? idUsuarioAlteracao { get; set; }

		public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }

        public string motivoExclusao { get; set; }
	}

	//
	internal sealed class PessoaTelefoneMapper : EntityTypeConfiguration<PessoaTelefone> {

		public PessoaTelefoneMapper() {

			this.ToTable("tb_pessoa_telefone");

            this.HasKey(o => o.id);

			this.HasRequired(o => o.Pessoa).WithMany(x => x.listaTelefones).HasForeignKey(o => o.idPessoa);
			this.HasOptional(o => o.TipoTelefone).WithMany().HasForeignKey(o => o.idTipoTelefone);
            this.HasOptional(o => o.OperadoraTelefonia).WithMany().HasForeignKey(o => o.idOperadoraTelefonia);
		}
	}
}