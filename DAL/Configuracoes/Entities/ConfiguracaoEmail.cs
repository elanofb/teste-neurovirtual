using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Notificacoes;
using DAL.Organizacoes;
using DAL.Permissao;

namespace DAL.Configuracoes {

	//
	public class ConfiguracaoEmail  {

		public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public Organizacao Organizacao { get; set; }
		
		public byte? idGatewayNotificacao { get; set; }

		public GatewayNotificacao GatewayNotificacao { get; set; }

        public string masterpageEmail { get; set; }

		public string assinaturaEmail { get; set; }

		public string contaEmailSistema { get; set; }

		public string senhaEmailSistema { get; set; }

		public string servidorPOPEmailSistema { get; set; }

		public string flagSSLPOPEmailSistema { get; set; }

		public int portaPOPEmailSistema { get; set; }

		public string servidorSMTPEmailSistema { get; set; }

		public string flagSSLSMTPEmailSistema { get; set; }

		public int portaSMTPEmailSistema { get; set; }
		
		public int? qtdeLimite { get; set; }
		
		public string chaveIntegracao { get; set; }
		
		public string emailResposta { get; set; }

        public DateTime dtCadastro { get; set; }

        public int idUsuarioCadastro { get; set; }

        public virtual UsuarioSistema UsuarioCadastro { get; set; }

        public bool? flagExcluido { get; set; }
	}

	//
	internal sealed class ConfiguracaoEmailMapper : EntityTypeConfiguration<ConfiguracaoEmail> {

		public ConfiguracaoEmailMapper() {
			
			this.ToTable("systb_configuracao_email");
			
			this.HasKey(o => o.id);

			this.HasRequired(x => x.UsuarioCadastro).WithMany().HasForeignKey(x => x.idUsuarioCadastro);
			
            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);
			
			this.HasOptional(x => x.GatewayNotificacao).WithMany().HasForeignKey(x => x.idGatewayNotificacao);
			
        }
	}
}