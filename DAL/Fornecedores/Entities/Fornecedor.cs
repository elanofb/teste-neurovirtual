using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using DAL.Organizacoes;
using DAL.Pessoas;

namespace DAL.Fornecedores {

	public class Fornecedor {

	    public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

		public int idPessoa { get; set; }

		public virtual Pessoa Pessoa { get; set; }

	    public DateTime dtCadastro { get; set; }

	    public DateTime dtAlteracao { get; set; }

	    public int idUsuarioCadastro { get; set; }

	    public int? idUsuarioAlteracao { get; set; }

	    public bool? ativo { get; set; }

	    public bool? flagExcluido { get; set; }

	}

	public class FornecedorMapper : EntityTypeConfiguration<Fornecedor> {

		public FornecedorMapper() {
			
			this.ToTable("tb_fornecedor");
	
			this.HasKey(x => x.id);

            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

			this.HasRequired(o => o.Pessoa).WithMany().HasForeignKey(o => o.idPessoa);
		}
	}
}