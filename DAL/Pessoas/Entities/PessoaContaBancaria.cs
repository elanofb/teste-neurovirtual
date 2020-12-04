using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using DAL.Bancos;
using DAL.Localizacao;
using DAL.Organizacoes;

namespace DAL.Pessoas {

	public class PessoaContaBancaria {
        
		public int id { get; set; }

	    public int? idOrganizacao { get; set; }
		
        public Organizacao Organizacao { get; set; }
		
		public int idPessoa { get; set; }
		
		public Pessoa Pessoa { get; set; }
		
		public int? idBanco { get; set; }

		public Banco Banco { get; set; }
								
        public string nroAgencia { get; set; }
		
        public string nroDigitoAgencia { get; set; }
		
        public string nroContaBancaria { get; set; }		                

        public bool? flagContaCorrente { get; set; }

		public bool? flagContaPoupanca { get; set; }
		
		public string nroDocumentoTitular { get; set; }
				
		public string nomeTitular { get; set; }
		
		public bool? ativo { get; set; }
				
		public DateTime? dtCadastro { get; set; }
		
        public int? idUsuarioCadastro { get; set; }

		public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }
		
		public PessoaContaBancaria() {

		}
		
	}
	
	internal sealed class PessoaContaBancariaMapper : EntityTypeConfiguration<PessoaContaBancaria> {

		public PessoaContaBancariaMapper() {
		
			this.ToTable("tb_pessoa_conta_bancaria");
			this.HasKey(x => x.id);
            //FK
			
            //Ignorar            			
		    this.HasRequired(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);
			
			this.HasRequired(x => x.Pessoa).WithMany().HasForeignKey(x => x.idPessoa);

			this.HasOptional(x => x.Banco).WithMany().HasForeignKey(x => x.idBanco);
			
            
		}
	}
}