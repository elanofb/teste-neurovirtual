using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Associados;
using DAL.Bancos;
using DAL.Entities;
using DAL.Organizacoes;
using DAL.Permissao;
using DAL.Pessoas;

namespace DAL.Historicos {
	
	public class HistoricoAtualizacao {		
		
		public int id { get; set; }
		
		public int idOrganizacao { get; set; }
		
		public Organizacao Organizacao { get; set; }
		
		public int? idAssociado { get; set; }
		
		public Associado Associado { get; set; }
		
		public int? idNaoAssociado { get; set; }
		
		public Associado NaoAssociado { get; set; }

		public int idPessoa { get; set; }
		
		public Pessoa Pessoa { get; set; }				
		
		public DateTime? dtAtualizacao { get; set; }
		
		public string emailOrigem { get; set; }
		
		public string informacoes { get; set; }
		
		public string informacoesAnteriores { get; set; }
		
		public string browser { get; set; }
		
		public bool? flagAprovado { get; set; }
		
		public DateTime? dtAnalise { get; set; }
		
		public int? idUsuarioAnalise { get; set; }
		
		public UsuarioSistema UsuarioAnalise { get; set; }
		
		public DateTime? dtCadastro { get; set; }		
		
		public DateTime? dtExclusao { get; set; }
		
		public int? idUsuarioExclusao { get; set; }
		
	}
	
	internal sealed class HistoricoAtualizacaoMapper : EntityTypeConfiguration<HistoricoAtualizacao> {
				
		public HistoricoAtualizacaoMapper() {
			
			this.ToTable("tb_historico_atualizacao");
			
			this.HasKey(x => x.id);
			
			this.HasRequired(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);
			
			this.HasOptional(x => x.Associado).WithMany().HasForeignKey(x => x.idAssociado);
			
			this.HasOptional(x => x.NaoAssociado).WithMany().HasForeignKey(x => x.idNaoAssociado);
			
			this.HasRequired(x => x.Pessoa).WithMany().HasForeignKey(x => x.idPessoa);
			
			this.HasOptional(x => x.UsuarioAnalise).WithMany().HasForeignKey(x => x.idUsuarioAnalise);									
			
		}		
		
	}
}