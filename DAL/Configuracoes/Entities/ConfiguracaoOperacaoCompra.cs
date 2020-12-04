using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;
using DAL.Permissao;

namespace DAL.Configuracoes {

	//
	public class ConfiguracaoOperacaoCompra {
	
		public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao {get; set;}

        public decimal? percentualLucro { get; set; }
		
		public decimal? percentualComissao { get; set; }
		
		public decimal? percentualCashback { get; set; }
		
		public decimal? percentualIndicacaoNivel1 { get; set; }
		
		public decimal? percentualIndicacaoNivel2 { get; set; }
		
		public decimal? percentualIndicacaoNivel3 { get; set; }
				
		public DateTime? dtCadastro { get; set; }
		
        public int? idUsuarioCadastro { get; set; }
		
        public UsuarioSistema UsuarioSistema { get; set; }
		
		public DateTime? dtExclusao { get; set; }
		
		public int? idUsuarioExclusao { get; set; }
		
		public UsuarioSistema UsuarioExclusao { get; set; }
        
	}

	//
	internal sealed class ConfiguracaoOperacaoCompraMapper : EntityTypeConfiguration<ConfiguracaoOperacaoCompra> {
		
		public ConfiguracaoOperacaoCompraMapper() {

			this.ToTable("tb_configuracao_operacao_compra");

            this.HasKey(o => o.id);
		   
            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);
			
			this.HasOptional(x => x.UsuarioSistema).WithMany().HasForeignKey(x => x.idUsuarioCadastro);
			
			this.HasOptional(x => x.UsuarioExclusao).WithMany().HasForeignKey(x => x.idUsuarioExclusao);
			
		}
	}
}