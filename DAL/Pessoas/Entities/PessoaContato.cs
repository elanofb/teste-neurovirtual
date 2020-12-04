using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Contatos;

namespace DAL.Pessoas {

	//
	public class PessoaContato {

		public int id { get; set; }

		public int? idOrganizacao { get; set; }
		
		public int? idTipoContato { get; set; }

		public virtual TipoContatoVW TipoContato { get; set; }

		public int idPessoa { get; set; }

		public virtual Pessoa Pessoa { get; set; }

		public string nome { get; set; }

		public string email { get; set; }

		public string telComercial { get; set; }

		public string telCelular { get; set; }
        
        public string observacao { get; set; }

		public DateTime? dtCadastro { get; set; }

		public DateTime? dtAlteracao { get; set; }

		public int? idUsuarioCadastro { get; set; }

		public int? idUsuarioAlteracao { get; set; }

		public string ativo { get; set; }

		public string flagExcluido { get; set; }
	}

	//
	internal sealed class PessoaContatoMapper : EntityTypeConfiguration<PessoaContato> {

		public PessoaContatoMapper() {
			this.ToTable("tb_pessoa_contato");
			this.HasKey(o => o.id);

			//FKs
			this.HasOptional(x => x.TipoContato).WithMany().HasForeignKey(o => o.idTipoContato);
			
		}
	}
}