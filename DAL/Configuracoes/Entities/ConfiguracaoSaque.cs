using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;
using DAL.Permissao;

namespace DAL.Configuracoes {

	//
	public class ConfiguracaoSaque {
		
		public int id { get; set; }
		
        public int? idOrganizacao { get; set; }
			
        public Organizacao Organizacao {get; set;}
		
		public int? idTipoCadastro { get; set; }					
		
		public decimal? valorMinimoSaque { get; set; }

        public bool? flagSegunda { get; set; }
		
		public string horarioInicioSegunda { get; set; }
		
		public string horarioFimSegunda { get; set; }
		
		public bool? flagTerca { get; set; }
		
		public string horarioInicioTerca { get; set; }
		
		public string horarioFimTerca { get; set; }
		
		public bool? flagQuarta { get; set; }
		
		public string horarioInicioQuarta { get; set; }
		
		public string horarioFimQuarta { get; set; }
		
		public bool? flagQuinta { get; set; }
		
		public string horarioInicioQuinta { get; set; }
		
		public string horarioFimQuinta { get; set; }
		
		public bool? flagSexta { get; set; }
		
		public string horarioInicioSexta { get; set; }
		
		public string horarioFimSexta { get; set; }
		
		public bool? flagSabado { get; set; }
		
		public string horarioInicioSabado { get; set; }
		
		public string horarioFimSabado { get; set; }
		
		public bool? flagDomingo { get; set; }
		
		public string horarioInicioDomingo { get; set; }
		
		public string horarioFimDomingo { get; set; }
		
		public DateTime? dtCadastro { get; set; }
		
        public int? idUsuarioCadastro { get; set; }
		
        public UsuarioSistema UsuarioSistema { get; set; }
		
		public DateTime? dtExclusao { get; set; }
		
		public int? idUsuarioExclusao { get; set; }
		
		public UsuarioSistema UsuarioExclusao { get; set; }
        
	}
		
	//
	internal sealed class ConfiguracaoSaqueMapper : EntityTypeConfiguration<ConfiguracaoSaque> {
			
		public ConfiguracaoSaqueMapper() {

			this.ToTable("tb_configuracao_saque");
			
            this.HasKey(o => o.id);
		   
            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);
			
			this.HasOptional(x => x.UsuarioSistema).WithMany().HasForeignKey(x => x.idUsuarioCadastro);
			
			this.HasOptional(x => x.UsuarioExclusao).WithMany().HasForeignKey(x => x.idUsuarioExclusao);
			
		}
	}
}
