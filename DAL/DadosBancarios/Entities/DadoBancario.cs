using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Bancos;
using DAL.Entities;
using DAL.Organizacoes;
using DAL.Pessoas;

namespace DAL.DadosBancarios {
	
	public class DadoBancario {		
		
		public int id { get; set; }
		
		public int idOrganizacao { get; set; }
		
		public Organizacao Organizacao { get; set; }

		public int idPessoa { get; set; }
		
		public Pessoa Pessoa { get; set; }
		
		public int idBanco { get; set; }
		
		public Banco Banco { get; set; }
		
		public string flagTipoConta { get; set; }
		
		public string nroAgencia { get; set; }
		
		public string nroConta { get; set; }
		
		public string digitoConta { get; set; }
		
		public string nomeTitular { get; set; }
		
		public string documentoTitular { get; set; }
		
		public string observacoes { get; set; }
		
		public bool? ativo { get; set; }
		
		public DateTime? dtCadastro { get; set; }
		
		public int? idUsuarioCadastro { get; set; }
		
		public DateTime? dtExclusao { get; set; }
		
		public int? idUsuarioExclusao { get; set; }
		
	}
	
	internal sealed class DadoBancarioMapper : EntityTypeConfiguration<DadoBancario> {
				
		public DadoBancarioMapper() {
			this.ToTable("tb_dado_bancario");
			this.HasKey(x => x.id);
			
			this.HasRequired(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);
			
			this.HasRequired(x => x.Pessoa).WithMany().HasForeignKey(x => x.idPessoa);
			
			this.HasRequired(x => x.Banco).WithMany().HasForeignKey(x => x.idBanco);
			
			
		}		
		
	}
}